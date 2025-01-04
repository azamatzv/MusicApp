using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Application.Services.Impl;

public class CardTypeService : ICardTypeService
{
    private readonly ICardTypeRepository _cardTypeRepository;
    public CardTypeService(ICardTypeRepository cardTypeRepository)
    {
        _cardTypeRepository = cardTypeRepository;
    }

    public async Task<CardTypeDto> AddCardTypeAsync(CardTypeDto cardTypeDto)
    {
        var cardType = new CardType
        {
            Name = cardTypeDto.Name,
            CreatedOn = DateTime.UtcNow
        };

        var addedCardType = await _cardTypeRepository.AddAsync(cardType);
        return new CardTypeDto
        {
            Name = addedCardType.Name
        };
    }

    public async Task<CardTypeDto> DeleteCardTypeAsync(Guid id)
    {
        var cardType = await _cardTypeRepository.GetFirstAsync(x => x.Id == id);
        var deletedCardType = await _cardTypeRepository.DeleteAsync(cardType);

        return new CardTypeDto
        {
            Name = deletedCardType.Name
        };
    }

    public async Task<List<CardTypeDto>> GetAllAsync()
    {
        var cardTypes = await _cardTypeRepository.GetAllAsync(x => true);
        return cardTypes.Select(ct => new CardTypeDto
        {
            Name = ct.Name
        }).ToList();
    }

    public async Task<CardTypeDto> GetByIdAsync(Guid id)
    {
        var cardType = await _cardTypeRepository.GetFirstAsync(x => x.Id == id);
        return new CardTypeDto
        {
            Name = cardType.Name
        };
    }

    public async Task<CardTypeDto> UpdateCardTypeAsync(Guid id, CardTypeDto CardTypeDto)
    {
        var existingCardType = await _cardTypeRepository.GetFirstAsync(x => x.Id == id);

        existingCardType.Name = CardTypeDto.Name;
        existingCardType.UpdatedOn = DateTime.UtcNow;

        var updatedCardType = await _cardTypeRepository.UpdateAsync(existingCardType);
        return new CardTypeDto
        {
            Name = updatedCardType.Name
        };
    }
}
