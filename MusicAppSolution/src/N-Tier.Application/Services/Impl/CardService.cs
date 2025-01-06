using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class CardService : ICardsService
{
    private readonly ICardsRepository _cardsRepository;
    private readonly ICardTypeRepository _cardTypeRepository;
    public CardService(ICardsRepository cardRepository, ICardTypeRepository cardTypeRepository)
    {
        _cardsRepository = cardRepository;
        _cardTypeRepository = cardTypeRepository;
    }

    private async Task<Guid> DetermineCardType(long cardNumber)
    {
        string cardPrefix = cardNumber.ToString().Substring(0, 4);

        var cardTypes = await _cardTypeRepository.GetAllAsync(x => true);

        // Determine card type based on prefix
        Guid cardTypeId = cardPrefix switch
        {
            // Uzcard starts with 8600
            var prefix when prefix.StartsWith("8600") =>
                cardTypes.First(ct => ct.Name.ToLower() == "UzCard").Id,

            // Humo starts with 9860
            var prefix when prefix.StartsWith("9860") =>
                cardTypes.First(ct => ct.Name.ToLower() == "humo").Id,

            // If no match found, throw an exception
            _ => throw new InvalidOperationException("Unknown card type based on card number prefix")
        };

        return cardTypeId;
    }

    public async Task<CardDto> AddCardAsync(CardDto cardDto)
    {
        // Validate card number
        if (cardDto.CardNumber.ToString().Length != 16)
        {
            throw new InvalidOperationException("Card number must be 16 digits");
        }

        // Determine card type based on first 4 digits
        var cardTypeId = await DetermineCardType(cardDto.CardNumber);

        var card = new Cards
        {
            UserId = cardDto.UserId,
            CardNumber = cardDto.CardNumber,
            CardTypeId = cardTypeId, // Set the determined card type
            Expire_Date = cardDto.Expire_Date,
            CreatedOn = DateTime.UtcNow
        };

        var addedCard = await _cardsRepository.AddAsync(card);
        return new CardDto
        {
            UserId = addedCard.UserId,
            CardNumber = addedCard.CardNumber,
            Expire_Date = addedCard.Expire_Date
        };
    }

    public async Task<bool> DeleteCardAsync(Guid id)
    {
        try
        {
            var card = await _cardsRepository.GetFirstAsync(x => x.Id == id);
            await _cardsRepository.DeleteAsync(card);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<CardDto>> GetAllAsync()
    {
        var cards = await _cardsRepository.GetAllAsync(x => true);
        return cards.Select(c => new CardDto
        {
            UserId = c.UserId,
            CardNumber = c.CardNumber,
            Expire_Date = c.Expire_Date
        }).ToList();
    }

    public async Task<CardDto> GetByIdAsync(Guid id)
    {
        var card = await _cardsRepository.GetFirstAsync(x => x.Id == id);
        return new CardDto
        {
            UserId = card.UserId,
            CardNumber = card.CardNumber,
            Expire_Date = card.Expire_Date
        };
    }

    public async Task<CardDto> UpdateCardAsync(Guid id, CardDto updateCardDto)
    {
        var existingCard = await _cardsRepository.GetFirstAsync(x => x.Id == id);

        existingCard.UserId = updateCardDto.UserId;
        existingCard.CardNumber = updateCardDto.CardNumber;
        existingCard.Expire_Date = updateCardDto.Expire_Date;
        existingCard.UpdatedOn = DateTime.UtcNow;

        var updatedCard = await _cardsRepository.UpdateAsync(existingCard);
        return new CardDto
        {
            UserId = updatedCard.UserId,
            CardNumber = updatedCard.CardNumber,
            Expire_Date = updatedCard.Expire_Date
        };
    }
}
