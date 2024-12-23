using N_Tier.Core.DTOs;

namespace N_Tier.Application.Services;

public interface ICardsService
{
    Task<CardDto> GetByIdAsync(Guid id);
    Task<List<CardDto>> GetAllAsync();
    Task<CardDto> AddCardAsync(CardDto userDto);
    Task<CardDto> UpdateCardAsync(Guid id, CardDto userDto);
    Task<bool> DeleteCardAsync(Guid id);
}