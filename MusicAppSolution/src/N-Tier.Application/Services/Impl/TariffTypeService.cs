using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class TariffTypeService : ITariffTypeService
{
    private readonly ITariffTypeRepository _repository;

    public TariffTypeService(ITariffTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<TariffType> AddTariffAsync(TariffTypeDto tariffTypeDto)
    {
        var tariff = new TariffType
        {
            Name = tariffTypeDto.Name,
            Amount = tariffTypeDto.Amount,
        };
        await _repository.AddAsync(tariff);
        return MapToDto(tariff);
    }

    private TariffType MapToDto(TariffType tariffType)
    {
        return new TariffType
        {
            Name = tariffType.Name,
            Amount = tariffType.Amount,
        };
    }
}
