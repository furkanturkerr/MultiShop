using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.OrderDtos.OrderAddressDtos;
using MultiShop.Dtos.OrderDtos.OrderDetailDtos;
using MultiShop.Dtos.OrderDtos.OrderingDtos;
using MultiShop.WebUI.Models.Order;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderAddressService _orderAddressService;
    private readonly IOrderService _orderService;
    private readonly IBasketService _basketService;

    public OrderController(
        IOrderAddressService orderAddressService,
        IOrderService orderService,
        IBasketService basketService)
    {
        _orderAddressService = orderAddressService;
        _orderService = orderService;
        _basketService = basketService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var basket = await _basketService.GetBasketAsync();
        if (basket.BasketItems.Count == 0)
        {
            TempData["CartError"] = "Ödeme adımına geçmek için sepetinizde ürün bulunmalıdır.";
            return RedirectToAction("Index", "Cart");
        }

        var model = new OrderCheckoutViewModel
        {
            Address = new CreateOrderAddressDto
            {
                Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                Country = "Türkiye"
            }
        };

        await PopulateSavedAddressesAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(OrderCheckoutViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var basket = await _basketService.GetBasketAsync();
        await PopulateSavedAddressesAsync(model, userId);

        if (string.IsNullOrWhiteSpace(userId))
            ModelState.AddModelError(string.Empty, "Kullanıcı bilgisi alınamadı. Lütfen yeniden giriş yapın.");

        if (basket.BasketItems.Count == 0)
            ModelState.AddModelError(string.Empty, "Sipariş oluşturmak için sepetinizde ürün bulunmalıdır.");

        ResultOrderAddressDto? selectedAddress = null;
        if (model.SelectedAddressId > 0)
        {
            selectedAddress = model.SavedAddresses.FirstOrDefault(x => x.AddressId == model.SelectedAddressId);
            if (selectedAddress is null)
                ModelState.AddModelError(nameof(model.SelectedAddressId), "Seçtiğiniz adres bulunamadı.");

            foreach (var key in ModelState.Keys.Where(x => x.StartsWith("Address.", StringComparison.Ordinal)).ToList())
                ModelState.Remove(key);
        }
        else
        {
            model.Address.UserId = userId;
        }

        ValidateCardExpiry(model);

        if (!ModelState.IsValid)
            return View(model);

        var addressId = selectedAddress?.AddressId;
        if (addressId is null)
        {
            addressId = await _orderAddressService.CreateOrderAddressAsync(model.Address);
            if (addressId is null)
            {
                ModelState.AddModelError(string.Empty, "Adres kaydedilemedi. Lütfen tekrar deneyin.");
                return View(model);
            }
        }

        var orderingId = await _orderService.CreateOrderingAsync(new CreateOrderingDto
        {
            AddressId = addressId.Value,
            PaymentMethod = "Kredi/Banka Kartı",
            TotalPrice = basket.TotalPriceAfterDiscount
        });

        if (orderingId is null)
        {
            ModelState.AddModelError(string.Empty, "Sipariş oluşturulamadı. Lütfen tekrar deneyin.");
            return View(model);
        }

        foreach (var item in basket.BasketItems)
        {
            var created = await _orderService.CreateOrderDetailAsync(new CreateOrderDetailDto
            {
                OrderingId = orderingId.Value,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductPrice = item.ProductPrice,
                ProductAmount = item.Quantity,
                ProductTotalPrice = item.ProductPrice * item.Quantity
            });

            if (!created)
            {
                ModelState.AddModelError(string.Empty, "Sipariş ürünleri kaydedilemedi. Sepetiniz korunmuştur.");
                return View(model);
            }
        }

        await _basketService.DeleteBasketAsync(userId);
        TempData["CompletedOrderingId"] = orderingId.Value;
        return RedirectToAction(nameof(Completed), new { id = orderingId.Value });
    }

    [HttpGet]
    public IActionResult Completed(int id)
    {
        if (id <= 0 || TempData["CompletedOrderingId"] is not int completedId || completedId != id)
            return RedirectToAction("Index", "Home");

        return View(id);
    }

    private async Task PopulateSavedAddressesAsync(OrderCheckoutViewModel model, string? userId = null)
    {
        userId ??= User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        model.SavedAddresses = (await _orderAddressService.GetAllOrderAddressesAsync())
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.AddressId)
            .ToList();

        if (model.SelectedAddressId == 0 && model.SavedAddresses.Count > 0 && IsAddressFormEmpty(model.Address))
            model.SelectedAddressId = model.SavedAddresses[0].AddressId;
    }

    private static bool IsAddressFormEmpty(CreateOrderAddressDto address) =>
        string.IsNullOrWhiteSpace(address.Name) &&
        string.IsNullOrWhiteSpace(address.Surname) &&
        string.IsNullOrWhiteSpace(address.Phone) &&
        string.IsNullOrWhiteSpace(address.City) &&
        string.IsNullOrWhiteSpace(address.District) &&
        string.IsNullOrWhiteSpace(address.Detail1);

    private void ValidateCardExpiry(OrderCheckoutViewModel model)
    {
        if (!int.TryParse(model.ExpiryMonth, out var month) ||
            !int.TryParse(model.ExpiryYear, out var shortYear) ||
            month is < 1 or > 12)
            return;

        var year = 2000 + shortYear;
        var lastValidDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        if (lastValidDay < DateTime.UtcNow.Date)
            ModelState.AddModelError(nameof(model.ExpiryYear), "Kartın son kullanma tarihi geçmiş.");
    }
}
