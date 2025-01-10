using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs.CardDtos;
using N_Tier.Core.Exceptions;

namespace MusicApp.Controllers
{
    [Authorize(Policy = "User")]
    [Route("api/cards")]
    public class CardsController : ApiControllerBase
    {
        private readonly ICardsService _cardsService;

        public CardsController(ICardsService cardsService)
        {
            _cardsService = cardsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CardDto>>> GetAll()
        {
            var cards = await _cardsService.GetAllAsync();
            return Ok(cards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CardDto>> GetById(Guid id)
        {
            try
            {
                var card = await _cardsService.GetByIdAsync(id);
                return Ok(card);
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CardDto>> Create([FromBody] CardDto cardDto)
        {
            try
            {
                var userId = GetUserIdFromToken();
                cardDto.UserId = userId;
                var createdCard = await _cardsService.AddCardAsync(cardDto);
                return CreatedAtAction(nameof(GetById), new { id = createdCard.UserId }, createdCard);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _cardsService.DeleteCardAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
