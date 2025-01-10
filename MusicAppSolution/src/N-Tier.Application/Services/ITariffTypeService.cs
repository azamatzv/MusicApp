using N_Tier.Core.DTOs.TariffTypeDtos;
using N_Tier.Core.Entities;

namespace N_Tier.Application.Services;

public interface ITariffTypeService
{
    Task<TariffTypeResponseDto> AddTariffAsync(TariffTypeDto tariffTypeDto);
    Task<TariffType?> GetTariffByIdAsync(Guid id);
    IEnumerable<TariffTypeResponseDto> GetTariffsAsync();
}
