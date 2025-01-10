using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs.CardTypeDtos;
using N_Tier.Core.Exceptions;

namespace MusicApp.Controllers
{
    [Route("api/card-types")]
    public class CardTypeController : ApiControllerBase
    {
        private readonly ICardTypeService _cardTypeService;

        public CardTypeController(ICardTypeService cardTypeService)
        {
            _cardTypeService = cardTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CardTypeDto>>> GetAll()
        {
            var cardTypes = await _cardTypeService.GetAllAsync();
            return Ok(cardTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CardTypeDto>> GetById(Guid id)
        {
            try
            {
                var cardType = await _cardTypeService.GetByIdAsync(id);
                return Ok(cardType);
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CardTypeDto>> Create([FromBody] CardTypeDto cardTypeDto)
        {
            var createdCardType = await _cardTypeService.AddCardTypeAsync(cardTypeDto);
            return Ok(createdCardType);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CardTypeDto>> Update(Guid id, [FromBody] CardTypeDto cardTypeDto)
        {
            try
            {
                var updatedCardType = await _cardTypeService.UpdateCardTypeAsync(id, cardTypeDto);
                return Ok(updatedCardType);
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CardTypeDto>> Delete(Guid id)
        {
            try
            {
                var deletedCardType = await _cardTypeService.DeleteCardTypeAsync(id);
                return Ok(deletedCardType);
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
