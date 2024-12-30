using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<GenreDto> AddGenreAsync(GenreDto userDto)
    {
        var genre = new Genre
        {
            Name = userDto.Name
        };

        var add = await _genreRepository.AddAsync(genre);

        return MapToDto(add);
    }

    public async Task<bool> DeleteGenreAsync(Guid id)
    {
        var result = await _genreRepository.GetFirstAsync(i => i.Id == id);
        if (result == null)
            throw new Exception("Genre not found");

        await _genreRepository.DeleteAsync(result);

        return true;
    }

    public async Task<List<GenreDto>> GetAllAsync()
    {
        var result = await _genreRepository.GetAllAsync(_ => true);

        return result.Select(MapToDto).ToList();
    }

    public async Task<GenreDto> GetByIdAsync(Guid id)
    {
        var result = await _genreRepository.GetFirstAsync(i => i.Id == id);
        if (result == null)
            throw new Exception("Genre not found");

        return MapToDto(result);
    }

    public async Task<GenreDto> UpdateGenreAsync(Guid id, GenreDto dto)
    {
        var result = await _genreRepository.GetFirstAsync(i => i.Id == id);
        if (result == null)
            throw new Exception("Genre not found");

        result.Name = dto.Name;

        var update = await _genreRepository.UpdateAsync(result);

        return MapToDto(update);
    }

    private GenreDto MapToDto(Genre genre)
    {
        return new GenreDto
        {
            Name = genre.Name
        };
    }
}