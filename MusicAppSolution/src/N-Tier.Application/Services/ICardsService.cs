using N_Tier.Core.DTOs.CardDtos;

namespace N_Tier.Application.Services;

public interface ICardsService
{
    Task<List<CardDto>> GetByIdAsync(Guid userId);
    Task<List<CardDto>> GetAllAsync();
    Task<CardDto> AddCardAsync(CardDto cardDto);
    Task<bool> DeleteCardAsync(Guid id);
}
