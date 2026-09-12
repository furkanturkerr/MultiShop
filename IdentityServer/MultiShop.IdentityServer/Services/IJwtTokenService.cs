using MultiShop.IdentityServer.Dtos;
using MultiShop.IdentityServer.Models;

namespace MultiShop.IdentityServer.Services;

public interface IJwtTokenService
{
    TokenResponse CreateToken(AppUser user, IList<string> roles);
}