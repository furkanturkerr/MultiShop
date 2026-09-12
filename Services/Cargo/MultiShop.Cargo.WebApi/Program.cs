using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MultiShop.Cargo.Business.Abstract;
using MultiShop.Cargo.Business.Concrete;
using MultiShop.Cargo.DataAccess.Abstract;
using MultiShop.Cargo.DataAccess.Concrete;
using MultiShop.Cargo.DataAccess.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --------------------
// DATABASE
// --------------------

builder.Services.AddDbContext<CargoContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});


// --------------------
// DATA ACCESS
// --------------------

builder.Services.AddScoped<ICargoDetailDal, EfCargoDetailDal>();
builder.Services.AddScoped<ICargoCompanyDal, EfCargoCompanyDal>();
builder.Services.AddScoped<ICargoCustomerDal, EfCargoCustomerDal>();
builder.Services.AddScoped<ICargoOperationDal, EfCargoOperationDal>();


// --------------------
// BUSINESS
// --------------------

builder.Services.AddScoped<ICargoDetailService, CargoDetailManager>();
builder.Services.AddScoped<ICargoCompanyService, CargoCompanyManager>();
builder.Services.AddScoped<ICargoCustomerService, CargoCustomerManager>();
builder.Services.AddScoped<ICargoOperationService, CargoOperationManager>();


// --------------------
// AUTOMAPPER
// --------------------

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// --------------------
// JWT AUTHENTICATION
// --------------------

var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),

            NameClaimType = "name",
            RoleClaimType = "role",

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();


// --------------------
// PIPELINE
// --------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();