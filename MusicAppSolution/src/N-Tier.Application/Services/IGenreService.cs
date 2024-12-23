using N_Tier.Core.DTOs;

namespace N_Tier.Application.Services;

public interface IGenreService
{
    Task<GenreDto> GetByIdAsync(Guid id);
    Task<List<GenreDto>> GetAllAsync();
    Task<GenreDto> AddGenreAsync(GenreDto userDto);
    Task<GenreDto> UpdateGenreAsync(Guid id, GenreDto userDto);
    Task<bool> DeleteGenreAsync(Guid id);
}