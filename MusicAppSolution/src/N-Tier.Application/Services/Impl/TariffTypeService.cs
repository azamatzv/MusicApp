using N_Tier.Core.DTOs.TariffTypeDtos;
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

    public async Task<TariffTypeResponseDto> AddTariffAsync(TariffTypeDto tariffTypeDto)
    {
        var tariff = new TariffType
        {
            Name = tariffTypeDto.Name,
            Amount = tariffTypeDto.Amount,
        };
        await _repository.AddAsync(tariff);
        return new TariffTypeResponseDto
        {
            Id = tariff.Id,
            Name = tariff.Name,
            Amount = tariff.Amount
        };
    }

    public async Task<TariffType?> GetTariffByIdAsync(Guid id)
    {
        return await _repository.GetFirstAsync(a => a.Id == id);
    }

    public IEnumerable<TariffTypeResponseDto> GetTariffsAsync()
    {
        var tariffs = _repository.GetAllAsEnumurable();
        return tariffs.Select(t => new TariffTypeResponseDto
        {
            Id = t.Id,
            Name = t.Name,
            Amount = t.Amount
        });
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
