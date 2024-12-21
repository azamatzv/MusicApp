using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;

namespace N_Tier.Application.Services;

public interface ITariffTypeService
{
    Task<TariffType> AddTariffAsync(TariffTypeDto tariffTypeDto);
}
