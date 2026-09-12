using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.Business.Abstract;
using MultiShop.Cargo.Dto.CargoCustomerDtos;

namespace MultiShop.Cargo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CargoCustomersController : ControllerBase
{
    private readonly ICargoCustomerService _cargoCustomerService;

    public CargoCustomersController(ICargoCustomerService cargoCustomerService)
    {
        _cargoCustomerService = cargoCustomerService;
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