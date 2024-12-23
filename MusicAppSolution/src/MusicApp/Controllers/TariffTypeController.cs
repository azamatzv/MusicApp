using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs;

namespace MusicApp.Controllers;

[Route("api/tarifftype")]
public class TariffTypeController : ApiController
{
    private readonly ITariffTypeService _tariffTypeService;

    public TariffTypeController(ITariffTypeService tariffTypeService)
    {
        _tariffTypeService = tariffTypeService;
    }

    [HttpPost]
    public async Task<IActionResult> AddTariffAsync([FromBody] TariffTypeDto tariffTypeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var addedTariff = await _tariffTypeService.AddTariffAsync(tariffTypeDto);
            return Ok(new
            {
                message = "Tariff successfully added.",
                tariff = addedTariff
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while adding the tariff.", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        try
        {
            var tariff = await _tariffTypeService.GetByIdAsync(id);
            return Ok(tariff);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the tariff.", details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var tariffs = await _tariffTypeService.GetAllAsync();
            return Ok(tariffs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the tariffs.", details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTariffTypeAsync(Guid id, [FromBody] TariffTypeDto tariffTypeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var updatedTariff = await _tariffTypeService.UpdateTariffTypeAsync(id, tariffTypeDto);
            return Ok(new
            {
                message = "Tariff successfully updated.",
                tariff = updatedTariff
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the tariff.", details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTariffTypeAsync(Guid id)
    {
        try
        {
            var isDeleted = await _tariffTypeService.DeleteTariffTypeAsync(id);
            return Ok(new
            {
                message = "Tariff successfully deleted.",
                isDeleted
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the tariff.", details = ex.Message });
        }
    }
}