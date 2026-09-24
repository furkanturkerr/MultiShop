using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Account;
using MultiShop.WebUI.Models;

namespace MultiShop.WebUI.Controllers;

[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectAfterLogin(returnUrl);

        return View(new LoginViewModel { ReturnUrl = LocalReturnUrl(returnUrl) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        model.ReturnUrl = LocalReturnUrl(model.ReturnUrl);
        if (!ModelState.IsValid)
            return LoginError(model);

        try
        {
            var client = _httpClientFactory.CreateClient("IdentityApi");
            using var response = await client.PostAsJsonAsync("auth/login", new
            {
                Email = model.Email.Trim(),
                model.Password
            }, HttpContext.RequestAborted);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, response.StatusCode == HttpStatusCode.Unauthorized
                    ? "E-posta adresi veya şifre yanlış."
                    : response.StatusCode == HttpStatusCode.TooManyRequests
                        ? "Çok fazla giriş denemesi yaptın. Bir süre sonra tekrar dene."
                        : "Şu anda giriş yapılamıyor. Biraz sonra tekrar dene.");
                return LoginError(model);
            }

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(HttpContext.RequestAborted);
            var handler = new JwtSecurityTokenHandler();
            if (tokenResponse is null || !handler.CanReadToken(tokenResponse.AccessToken))
                throw new JsonException();

            // Claims come only from Identity's login response through the configured Gateway.
            // Never accept a token from a browser field or query parameter here.
            var token = handler.ReadJwtToken(tokenResponse.AccessToken);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var name = token.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var expiresAt = new DateTimeOffset(token.ValidTo, TimeSpan.Zero);
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(token.RawSignature) || expiresAt <= DateTimeOffset.UtcNow ||
                tokenResponse.ExpiresAt <= DateTimeOffset.UtcNow)
                throw new JsonException();

            if (tokenResponse.ExpiresAt < expiresAt)
                expiresAt = tokenResponse.ExpiresAt;

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Name, string.IsNullOrWhiteSpace(name) ? email : name)
            };
            claims.AddRange(token.Claims.Where(c => c.Type == "role")
                .Select(c => new Claim(ClaimTypes.Role, c.Value)));

            var properties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = expiresAt,
                AllowRefresh = false
            };
            properties.StoreTokens(new[]
            {
                new AuthenticationToken { Name = "access_token", Value = tokenResponse.AccessToken },
                new AuthenticationToken { Name = "expires_at", Value = expiresAt.ToString("o", CultureInfo.InvariantCulture) }
            });
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity), properties);

            return RedirectAfterLogin(model.ReturnUrl);
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or JsonException or ArgumentException)
        {
            ModelState.AddModelError(string.Empty, "Giriş servisine ulaşılamadı. Biraz sonra tekrar dene.");
            return LoginError(model);
        }
    }

    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectAfterLogin(returnUrl);

        return View(new RegisterViewModel { ReturnUrl = LocalReturnUrl(returnUrl) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        model.ReturnUrl = LocalReturnUrl(model.ReturnUrl);
        if (!ModelState.IsValid)
            return RegisterError(model);

        try
        {
            var client = _httpClientFactory.CreateClient("IdentityApi");
            using var response = await client.PostAsJsonAsync("auth/register", new
            {
                FullName = $"{model.Name.Trim()} {model.Surname.Trim()}",
                Email = model.Email.Trim(),
                model.Password
            }, HttpContext.RequestAborted);

            if (response.IsSuccessStatusCode)
            {
                TempData["AuthSuccess"] = "Hesabın oluşturuldu. E-posta adresin ve şifrenle giriş yapabilirsin.";
                return RedirectToAction(nameof(Login), new { returnUrl = model.ReturnUrl });
            }

            if (response.StatusCode == HttpStatusCode.Conflict)
                ModelState.AddModelError(nameof(model.Email), "Bu e-posta adresiyle zaten bir hesap var. Giriş yapabilirsin.");
            else
                ModelState.AddModelError(string.Empty, response.StatusCode == HttpStatusCode.BadRequest
                    ? "Kayıt oluşturulamadı. E-posta adresini ve şifre kurallarını kontrol et."
                    : "Kayıt servisi şu anda kullanılamıyor. Biraz sonra tekrar dene.");
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
        {
            ModelState.AddModelError(string.Empty, "Kayıt servisine ulaşılamadı. Biraz sonra tekrar dene.");
        }

        return RegisterError(model);
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        return View("~/Views/Shared/GatewayError.cshtml", new GatewayErrorViewModel { StatusCode = 403 });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    private string? LocalReturnUrl(string? returnUrl) => Url.IsLocalUrl(returnUrl) ? returnUrl : null;

    private IActionResult RedirectAfterLogin(string? returnUrl) => LocalReturnUrl(returnUrl) is { } localUrl
        ? LocalRedirect(localUrl)
        : RedirectToAction("Index", "Home", new { area = "" });

    private ViewResult LoginError(LoginViewModel model)
    {
        model.Password = string.Empty;
        ModelState.SetModelValue(nameof(model.Password), string.Empty, string.Empty);
        return View(nameof(Login), model);
    }

    private ViewResult RegisterError(RegisterViewModel model)
    {
        model.Password = model.ConfirmPassword = string.Empty;
        ModelState.SetModelValue(nameof(model.Password), string.Empty, string.Empty);
        ModelState.SetModelValue(nameof(model.ConfirmPassword), string.Empty, string.Empty);
        return View(nameof(Register), model);
    }

    private sealed class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
