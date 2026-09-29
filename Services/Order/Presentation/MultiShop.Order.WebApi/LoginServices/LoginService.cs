using System.Security.Claims;

namespace MultiShop.Order.WebApi.LoginServices;

public class LoginService : ILoginService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException(
                    "Kullanıcı ID bilgisi token içerisinde bulunamadı.");
            }

            return userId;
        }
    }
}
