using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiShop.IdentityServer.Dtos;
using MultiShop.IdentityServer.Models;
using MultiShop.IdentityServer.Services;

namespace MultiShop.IdentityServer.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
            return Conflict("Bu email zaten kayıtlı.");

        var user = new AppUser
        {
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(x => x.Description));
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Unauthorized("Email veya şifre yanlış.");

        var result =
            await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
            return Unauthorized("Email veya şifre yanlış.");

        var roles =
            await _userManager.GetRolesAsync(user);

        var token =
            _jwtTokenService.CreateToken(user, roles);

        return Ok(token);
    }
}