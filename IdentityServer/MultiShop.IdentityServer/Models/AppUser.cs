using Microsoft.AspNetCore.Identity;

namespace MultiShop.IdentityServer.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = null!;
}