using Microsoft.AspNetCore.Authentication.Cookies;
using MultiShop.WebUI.Infrastructure.Gateway;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var gatewayUrl = builder.Configuration["GatewayApi:BaseUrl"];
if (!Uri.TryCreate(gatewayUrl, UriKind.Absolute, out var gatewayAddress) ||
    !gatewayAddress.AbsolutePath.EndsWith('/') || !string.IsNullOrEmpty(gatewayAddress.Query) ||
    !string.IsNullOrEmpty(gatewayAddress.Fragment) || !string.IsNullOrEmpty(gatewayAddress.UserInfo))
    throw new InvalidOperationException("GatewayApi:BaseUrl geçerli ve / ile biten bir adres olmalı.");
if (gatewayAddress.Scheme != Uri.UriSchemeHttps &&
    !(builder.Environment.IsDevelopment() && gatewayAddress.IsLoopback && gatewayAddress.Scheme == Uri.UriSchemeHttp))
    throw new InvalidOperationException("Gateway bağlantısı HTTPS kullanmalı.");

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<GatewayTokenHandler>();
builder.Services.AddHttpClient("GatewayApi", client =>
{
    client.BaseAddress = new Uri(gatewayAddress, "services/");
    client.Timeout = TimeSpan.FromSeconds(15);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    AllowAutoRedirect = false,
    UseCookies = false
}).AddHttpMessageHandler<GatewayTokenHandler>();

builder.Services.AddHttpClient("IdentityApi", client =>
{
    client.BaseAddress = new Uri(gatewayAddress, "services/identity/");
    client.Timeout = TimeSpan.FromSeconds(15);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    AllowAutoRedirect = false,
    UseCookies = false
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "MultiShop.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.SlidingExpiration = false;
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<GatewayExceptionMiddleware>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
