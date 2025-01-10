using N_Tier.Application.Helper;
using N_Tier.Core.DTOs.CardDtos;
using N_Tier.Core.Entities;
using N_Tier.Core.Exceptions;
using N_Tier.DataAccess.Repositories;
using System.Globalization;

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

        Guid cardTypeId = cardPrefix switch
        {
            var prefix when prefix.StartsWith("8600") =>
                cardTypes.First(ct => ct.Name.ToLower() == PlasticCardTypesConst.UZCARD).Id,

            var prefix when prefix.StartsWith("9860") =>
                cardTypes.First(ct => ct.Name.ToLower() == PlasticCardTypesConst.HUMO).Id,

            _ => throw new InvalidOperationException("Unknown card type based on card number prefix")
        };

        return cardTypeId;
    }

    public async Task<CardDto> AddCardAsync(CardDto cardDto)
    {
        if (cardDto.CardNumber.ToString().Length != 16)
        {
            throw new InvalidOperationException("Card number must be 16 digits");
        }

        var existingCard = await _cardsRepository.GetFirstExistCardAsync(x => x.CardNumber == cardDto.CardNumber);
        if (existingCard != null)
        {
            throw new InvalidOperationException("This card number already exists");
        }

        // Expire date validatsiyasi
        ValidateExpireDate(cardDto.Expire_Date);
        var normalizedExpireDate = NormalizeDateTime(cardDto.Expire_Date);

        var cardTypeId = await DetermineCardType(cardDto.CardNumber);

        var card = new Cards
        {
            UserId = cardDto.UserId,
            CardNumber = cardDto.CardNumber,
            CardTypeId = cardTypeId,
            Expire_Date = normalizedExpireDate,
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

    public async Task<List<CardDto>> GetByIdAsync(Guid userId)
    {
        var cards = await _cardsRepository.GetAllAsync(x => x.UserId == userId);

        return cards.Select(card => new CardDto
        {
            UserId = card.UserId,
            CardNumber = card.CardNumber,
            Expire_Date = card.Expire_Date
        }).ToList();
    }


    private bool ValidateExpireDate(DateTime expireDate)
    {
        // Kunni 1-kunga o'rnatamiz (faqat oy va yil muhim)
        var normalizedExpireDate = new DateTime(expireDate.Year, expireDate.Month, 1);

        // Hozirgi oyning 1-kuni bilan solishtiramiz
        var currentDate = DateTime.Now;
        var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

        if (normalizedExpireDate < firstDayOfCurrentMonth)
            throw new InvalidOperationException("Expiration date cannot be in the past");

        // Kartaning maksimal amal qilish muddati (masalan 10 yil)
        var maxValidDate = firstDayOfCurrentMonth.AddYears(10);
        if (normalizedExpireDate > maxValidDate)
            throw new InvalidOperationException("Card expiration date cannot be more than 10 years in the future");

        return true;
    }

    private DateTime NormalizeDateTime(DateTime expireDate)
    {
        // Kunni 1-kunga o'rnatamiz, vaqtni 00:00:00 ga
        return new DateTime(expireDate.Year, expireDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
