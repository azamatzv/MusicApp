using N_Tier.Core.DTOs;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public Task<GenreDto> AddGenreAsync(GenreDto userDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteGenreAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<GenreDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GenreDto> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<GenreDto> UpdateGenreAsync(Guid id, GenreDto userDto)
    {
        throw new NotImplementedException();
    }
}