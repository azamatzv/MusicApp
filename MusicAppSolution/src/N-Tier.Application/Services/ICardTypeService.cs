using N_Tier.Core.DTOs;

namespace N_Tier.Application.Services;

public interface ICardTypeService
{
    Task<CardTypeDto> GetByIdAsync(Guid id);
    Task<List<CardTypeDto>> GetAllAsync();
    Task<CardTypeDto> AddCardTypeAsync(CardTypeDto cardTypeDto);
    Task<CardTypeDto> UpdateCardTypeAsync(Guid id, CardTypeDto CardTypeDto);
    Task<CardTypeDto> DeleteCardTypeAsync(Guid id);
}
