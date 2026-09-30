using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Basket.Dtos;
using MultiShop.Basket.LoginServices;
using MultiShop.Basket.Settings;

namespace MultiShop.Basket.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BasketsController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly ILoginService _loginService;

    public BasketsController(IBasketService basketService, ILoginService loginService)
    {
        _basketService = basketService;
        _loginService = loginService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyBasketDetail()
    {
        try
        {
            return Ok(await _basketService.GetBasketAsync(_loginService.GetUserId));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "Ürün veya kupon servisine ulaşılamıyor.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveMyBasket(BasketTotalDto basketTotalDto)
    {
        basketTotalDto.UserId = _loginService.GetUserId;

        try
        {
            await _basketService.SaveBasketAsync(basketTotalDto);
            return Ok();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "Ürün veya kupon servisine ulaşılamıyor.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMyBasket()
    {
        await _basketService.DeleteBasketAsync(_loginService.GetUserId);
        return Ok();
    }
}
