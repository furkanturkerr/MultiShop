using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.Business.Abstract;
using MultiShop.Cargo.Dto.CargoDetailDtos;

namespace MultiShop.Cargo.WebApi.Controllers;

[Authorize(Roles = "Admin,Manager")]
[ApiController]
[Route("api/[controller]")]
public class CargoDetailsController : ControllerBase
{
    private readonly ICargoDetailService _cargoDetailService;

    public CargoDetailsController(ICargoDetailService cargoDetailService)
    {
        _cargoDetailService = cargoDetailService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _cargoDetailService.TGetAllAsync();
        return Ok(values);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _cargoDetailService.TGetByIdAsync(id);

        if (value is null)
            return NotFound();

        return Ok(value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCargoDetailDto dto)
    {
        await _cargoDetailService.TInsertAsync(dto);
        return Ok("Cargo detail created.");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCargoDetailDto dto)
    {
        await _cargoDetailService.TUpdateAsync(dto);
        return Ok("Cargo detail updated.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _cargoDetailService.TDeleteAsync(id);
        return Ok("Cargo detail deleted.");
    }
}