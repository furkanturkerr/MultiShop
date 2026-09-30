using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using MultiShop.IdentityServer.Models;
using MultiShop.IdentityServer.Services;
using MultiShop.IdentityServer.Settings;

namespace MultiShop.IdentityServer.Tests.Services;

public class JwtTokenServiceTests
{
    [Fact]
    public void CreateToken_KullaniciIdSiniSubClaimineYazar()
    {
        // Arrange
        var settings = new JwtSettings
        {
            Issuer = "test-identity",
            Audience = "test-api",
            Key = "test-only-signing-key-at-least-32-bytes-long",
            AccessTokenMinutes = 30
        };

        var service = new JwtTokenService(Options.Create(settings));

        var user = new AppUser
        {
            Id = "user-123",
            Email = "furkan@example.com",
            FullName = "Furkan Türker"
        };

        // Act
        var response = service.CreateToken(user, ["Customer"]);

        var token = new JwtSecurityTokenHandler().ReadJwtToken(response.AccessToken);

        var userId = token.Claims
            .Single(claim => claim.Type == "sub")
            .Value;

        // Assert
        Assert.Equal("user-123", userId);
    }
}