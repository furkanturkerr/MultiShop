using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.Business.Abstract;
using MultiShop.Cargo.Dto.CargoOperationDtos;

namespace MultiShop.Cargo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CargoOperationsController : ControllerBase
{
    private readonly ICargoOperationService _cargoOperationService;

    public CargoOperationsController(ICargoOperationService cargoOperationService)
    {
        _cargoOperationService = cargoOperationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _cargoOperationService.TGetAllAsync();
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _cargoOperationService.TGetByIdAsync(id);

        if (value is null)
            return NotFound();

        return Ok(value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCargoOperationDto dto)
    {
        await _cargoOperationService.TInsertAsync(dto);
        return Ok("Cargo operation created.");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCargoOperationDto dto)
    {
        await _cargoOperationService.TUpdateAsync(dto);
        return Ok("Cargo operation updated.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _cargoOperationService.TDeleteAsync(id);
        return Ok("Cargo operation deleted.");
    }
}