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

    public async Task<TariffTypeDto> AddTariffAsync(TariffTypeDto tariffTypeDto)
    {
        var tariff = new TariffType
        {
            Name = tariffTypeDto.Name,
            Amount = tariffTypeDto.Amount,
        };

        var add = await _repository.AddAsync(tariff);

        return MapToDto(add);
    }

    public async Task<bool> DeleteTariffTypeAsync(Guid id)
    {
        var result = await _repository.GetFirstAsync(i => i.Id == id);
        if (result == null)
            throw new Exception("TariffType not found");

        await _repository.DeleteAsync(result);

        return true;
    }

    public async Task<List<TariffTypeDto>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync(_ => true);
        return result.Select(MapToDto).ToList();
    }

    public async Task<TariffTypeDto> GetByIdAsync(Guid id)
    {
        var result = await _repository.GetFirstAsync(i => i.Id == id);
        if (result == null)
            throw new Exception("TariffType not found");

        return MapToDto(result);
    }

    public async Task<TariffTypeDto> UpdateTariffTypeAsync(Guid id, TariffTypeDto tariff)
    {
        var result = await _repository.GetFirstAsync(i => i.Id == id);
        if (result == null)
            throw new Exception("TariffType not found");

        result.Name = tariff.Name;
        result.Amount = tariff.Amount;

        await _repository.UpdateAsync(result);

        return MapToDto(result);
    }

    private TariffTypeDto MapToDto(TariffType tariffType)
    {
        return new TariffTypeDto
        {
            Name = tariffType.Name,
            Amount = tariffType.Amount,
        };
    }
}
