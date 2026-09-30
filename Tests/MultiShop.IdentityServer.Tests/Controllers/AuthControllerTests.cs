using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MultiShop.IdentityServer.Controllers;
using MultiShop.IdentityServer.Dtos;
using MultiShop.IdentityServer.Models;
using MultiShop.IdentityServer.Services;

namespace MultiShop.IdentityServer.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    //“Bu interface için sahte nesne oluştur ve doğrudan nesneyi ver”
    private readonly AuthController _controller;
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<IJwtTokenService> _tokenServiceMock;

    //sahte servisleri ve controller'ı hazırlar
    public AuthControllerTests()
    {
        _userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(),
            Options.Create(new IdentityOptions()),
            Mock.Of<IPasswordHasher<AppUser>>(),
            Array.Empty<IUserValidator<AppUser>>(),
            Array.Empty<IPasswordValidator<AppUser>>(),
            Mock.Of<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<AppUser>>>());

        _signInManagerMock = new Mock<SignInManager<AppUser>>(
            _userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            Options.Create(new IdentityOptions()),
            Mock.Of<ILogger<SignInManager<AppUser>>>(),
            Mock.Of<IAuthenticationSchemeProvider>(),
            Mock.Of<IUserConfirmation<AppUser>>());

        _tokenServiceMock = new Mock<IJwtTokenService>();

        _controller = new AuthController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _tokenServiceMock.Object);
    }
    
    [Fact]
    public async Task Login_Basariliysa_RollerleTokenUretirVeDoner()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "furkan@example.com",
            Password = "Example123!"
        };

        var user = new AppUser
        {
            Id = "user-123",
            Email = request.Email
        };

        IList<string> roles = new List<string> { "Customer" };

        var tokenResponse = new TokenResponse
        {
            AccessToken = "test-token",
            ExpiresAt = DateTime.UtcNow.AddMinutes(30)
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.CheckPasswordSignInAsync(
                user, request.Password, true))
            .ReturnsAsync(
                Microsoft.AspNetCore.Identity.SignInResult.Success);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _tokenServiceMock
            .Setup(x => x.CreateToken(user, roles))
            .Returns(tokenResponse);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(tokenResponse, okResult.Value);

        _tokenServiceMock.Verify(
            x => x.CreateToken(user, roles),
            Times.Once);
    }
    
    [Fact]
    public async Task Register_Basariliysa_CustomerRoluAtarVeOkDoner()
    {
        // Arrange
        var request = new RegisterRequest
        {
            FullName = "Furkan Türker",
            Email = "furkan@example.com",
            Password = "Example123!"
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((AppUser?)null);

        _userManagerMock
            .Setup(x => x.CreateAsync(
                It.IsAny<AppUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.AddToRoleAsync(
                It.IsAny<AppUser>(), "Customer"))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _controller.Register(request);

        // Assert
        Assert.IsType<OkResult>(result);

        _userManagerMock.Verify(
            x => x.CreateAsync(
                It.Is<AppUser>(user =>
                    user.Email == request.Email &&
                    user.UserName == request.Email &&
                    user.FullName == request.FullName),
                request.Password),
            Times.Once);

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(
                It.Is<AppUser>(user => user.Email == request.Email),
                //It.IsAny herhangi bir kullanıcıyı kabul ediyordu. It.Is ise koşula uyan kullanıcıyı arıyor. Böylece oluşturulan kullanıcının alanlarını kontrol ediyoruz.
                "Customer"),
            Times.Once);
    }
    
    [Fact]
    public async Task Login_SifreYanlissa_UnauthorizedDonerVeTokenUretmez()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "furkan@example.com",
            Password = "yanlis-sifre"
        };

        var user = new AppUser
        {
            Id = "user-123",
            Email = request.Email
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(x => x.CheckPasswordSignInAsync(
                user, request.Password, true))
            .ReturnsAsync(
                Microsoft.AspNetCore.Identity.SignInResult.Failed);

        // Act
        var result = await _controller.Login(request);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);

        _tokenServiceMock.Verify(
            x => x.CreateToken(
                It.IsAny<AppUser>(),
                It.IsAny<IList<string>>()),
            Times.Never);
    }
    
    
    [Fact]
    public async Task Register_EmailKayitliysa_ConflictDoner()
    {
        // Arrange
        var request = new RegisterRequest
        {
            FullName = "Furkan Türker",
            Email = "furkan@example.com",
            Password = "Example123!"
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(new AppUser
            {
                Email = request.Email
            });

        // Act
        var result = await _controller.Register(request);

        // Assert
        Assert.IsType<ConflictObjectResult>(result);

        _userManagerMock.Verify(
            x => x.CreateAsync(
                It.IsAny<AppUser>(),
                It.IsAny<string>()),
            Times.Never);
    }
    
    [Fact]
    public async Task Login_KullaniciYoksa_UnauthorizedDoner()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "olmayan@example.com",
            Password = "Example123!"
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((AppUser?)null);

        // Act
        var result = await _controller.Login(request);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}