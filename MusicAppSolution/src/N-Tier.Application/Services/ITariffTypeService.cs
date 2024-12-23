using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;

namespace N_Tier.Application.Services;

public interface ITariffTypeService
{
    Task<TariffTypeDto> GetByIdAsync(Guid id);
    Task<List<TariffTypeDto>> GetAllAsync();
    Task<TariffTypeDto> AddTariffAsync(TariffTypeDto tariff);
    Task<TariffTypeDto> UpdateTariffTypeAsync(Guid id, TariffTypeDto tariff);
    Task<bool> DeleteTariffTypeAsync(Guid id);
}
