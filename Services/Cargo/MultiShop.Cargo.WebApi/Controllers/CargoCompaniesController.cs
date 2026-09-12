using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.Business.Abstract;
using MultiShop.Cargo.Dto.CargoCompanyDtos;

namespace MultiShop.Cargo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CargoCompaniesController : ControllerBase
{
    private readonly ICargoCompanyService _cargoCompanyService;

    public CargoCompaniesController(ICargoCompanyService cargoCompanyService)
    {
        _cargoCompanyService = cargoCompanyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _cargoCompanyService.TGetAllAsync();
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _cargoCompanyService.TGetByIdAsync(id);

        if (value is null)
            return NotFound();

        return Ok(value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCargoCompanyDto dto)
    {
        await _cargoCompanyService.TInsertAsync(dto);
        return Ok("Cargo company created.");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCargoCompanyDto dto)
    {
        await _cargoCompanyService.TUpdateAsync(dto);
        return Ok("Cargo company updated.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _cargoCompanyService.TDeleteAsync(id);
        return Ok("Cargo company deleted.");
    }
}