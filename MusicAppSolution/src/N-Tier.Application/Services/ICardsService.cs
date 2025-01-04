using N_Tier.Core.DTOs;

namespace N_Tier.Application.Services;

public interface ICardsService
{
    Task<CardDto> GetByIdAsync(Guid id);
    Task<List<CardDto>> GetAllAsync();
    Task<CardDto> AddCardAsync(CardDto cardDto);
    Task<CardDto> UpdateCardAsync(Guid id, CardDto updateCardDto);
    Task<bool> DeleteCardAsync(Guid id);
}
