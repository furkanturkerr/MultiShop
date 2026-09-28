using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MultiShop.Basket.LoginServices;
using MultiShop.Basket.Settings;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<RedisService>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ILoginService, LoginService>();


builder.Services.Configure<RedisSettings>(
    builder.Configuration.GetSection("RedisSettings"));

var redisSettings = builder.Configuration
    .GetSection("RedisSettings")
    .Get<RedisSettings>();

if (redisSettings is null)
{
    throw new InvalidOperationException(
        "RedisSettings appsettings.json içerisinde bulunamadı.");
}

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    return ConnectionMultiplexer.Connect(
        $"{redisSettings.Host}:{redisSettings.Port}");
});

var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT Key appsettings.json içerisinde bulunamadı.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            NameClaimType = "name",
            RoleClaimType = "role",

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
