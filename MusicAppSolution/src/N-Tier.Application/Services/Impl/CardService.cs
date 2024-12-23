using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class CardService : ICardsService
{
    private readonly ICardsRepository _cardsRepository;

    public CardService(ICardsRepository cardsRepository)
    {
        _cardsRepository = cardsRepository;
    }

    public async Task<CardDto> AddCardAsync(CardDto userDto)
    {
        var card = new Cards
        {
            Expire_Date = userDto.Expire_Date,
            CardNumber = userDto.CardNumber,
            CardType = userDto.CardType
        };

        var result = await _cardsRepository.AddAsync(card);

        return MapToDto(result);
    }

    public async Task<bool> DeleteCardAsync(Guid id)
    {
        var result = await _cardsRepository.GetFirstAsync(i => i.Id == id);

        if (result == null)
            throw new Exception("User not found");

        await _cardsRepository.DeleteAsync(result);

        return true;
    }

    public async Task<List<CardDto>> GetAllAsync()
    {
        var result = await _cardsRepository.GetAllAsync(_ => true);
        return result.Select(MapToDto).ToList();
    }

    public async Task<CardDto> GetByIdAsync(Guid id)
    {
        var result = await _cardsRepository.GetFirstAsync(i => i.Id == id);
        if (result == null)
            throw new Exception("Card not found");

        return MapToDto(result);
    }

    public async Task<CardDto> UpdateCardAsync(Guid id, CardDto userDto)
    {
        var result = await _cardsRepository.GetFirstAsync(i => i.Id == id);
        if (result == null)
            throw new Exception("Card not found");

        result.Expire_Date = userDto.Expire_Date;
        result.CardNumber = userDto.CardNumber;
        result.CardType = userDto.CardType;

        await _cardsRepository.UpdateAsync(result);

        return MapToDto(result);
    }

    private CardDto MapToDto(Cards card)
    {
        return new CardDto
        {
            CardNumber = card.CardNumber,
            Expire_Date = card.Expire_Date,
            CardType = card.CardType,
        };
    }
}
