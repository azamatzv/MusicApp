using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;
using N_Tier.Application.Services.Impl;
using N_Tier.Core.DTOs.TariffTypeDtos;
using N_Tier.Core.Entities;

namespace MusicApp.Controllers
{

    //[Authorize]
    [Route("api/TariffType")]
    public class TariffTypeController : ApiControllerBase
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

        [HttpGet("GetAllTariffs")]
        public IActionResult GetAllTariffs()
        {
            try
            {
                var tariffs = _tariffTypeService.GetTariffsAsync();
                if (!tariffs.Any())
                {
                    return NotFound("Tariffs not found.");
                }
                return Ok(tariffs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
