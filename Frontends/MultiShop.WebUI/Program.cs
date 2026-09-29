using Microsoft.AspNetCore.Authentication.Cookies;
using MultiShop.WebUI.Infrastructure.Gateway;
using MultiShop.WebUI.Services.Concrete;
using MultiShop.WebUI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var gatewayUrl = builder.Configuration["GatewayApi:BaseUrl"]!;

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<GatewayTokenHandler>();
builder.Services.AddHttpClient("GatewayApi", client =>
{
    client.BaseAddress = new Uri($"{gatewayUrl}services/");
}).AddHttpMessageHandler<GatewayTokenHandler>();

builder.Services.AddHttpClient("IdentityApi", client =>
{
    client.BaseAddress = new Uri($"{gatewayUrl}services/identity/");
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IFeatureSliderService, FeatureSliderService>();
builder.Services.AddScoped<IFeaturedService, FeaturedService>();
builder.Services.AddScoped<IOfferDiscountService, OfferDiscountService>();
builder.Services.AddScoped<ISpecialOfferService, SpecialOfferService>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IOrderAddressService, OrderAddressService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICargoCompanyService, CargoCompanyService>();
builder.Services.AddScoped<ICargoCustomerService, CargoCustomerService>();

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
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
