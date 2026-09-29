using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.Business.Abstract;
using MultiShop.Cargo.Dto.CargoCustomerDtos;
using MultiShop.Cargo.WebApi.LoginServices;

namespace MultiShop.Cargo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CargoCustomersController : ControllerBase
{
    private readonly ICargoCustomerService _cargoCustomerService;
    private readonly ILoginService _loginService;

    public CargoCustomersController(ICargoCustomerService cargoCustomerService, ILoginService loginService)
    {
        _cargoCustomerService = cargoCustomerService;
        _loginService = loginService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _cargoCustomerService.TGetAllAsync();
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _cargoCustomerService.TGetByIdAsync(id);

        if (value is null)
            return NotFound();

        return Ok(value);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyCargoCustomer()
    {
        var value = await _cargoCustomerService.TGetByUserCustomerIdAsync(_loginService.GetUserId);
        return value is null ? NotFound() : Ok(value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCargoCustomerDto dto)
    {
        await _cargoCustomerService.TInsertAsync(dto);
        return Ok("Cargo customer created.");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCargoCustomerDto dto)
    {
        await _cargoCustomerService.TUpdateAsync(dto);
        return Ok("Cargo customer updated.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _cargoCustomerService.TDeleteAsync(id);
        return Ok("Cargo customer deleted.");
    }
}
