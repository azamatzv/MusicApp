using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorDto> AddAuthorAsync(AuthorDto dto)
    {
        var author = new Author
        {
            Name = dto.Name,
            CreatedBy = "System"
        };

        var add = await _authorRepository.AddAsync(author);

        return MapToDto(add);
    }

    public async Task<bool> DeleteAuthorAsync(Guid id)
    {
        var result = await _authorRepository.GetFirstAsync(i => i.Id == id);
        if (result == null)
            throw new Exception("Author not found");

        await _authorRepository.DeleteAsync(result);

        return true;
    }

    public async Task<List<AuthorDto>> GetAllAsync()
    {
        var result = await _authorRepository.GetAllAsync(_ => true);
        return result.Select(MapToDto).ToList();
    }

    public async Task<AuthorDto> GetByIdAsync(Guid id)
    {
        var result = await _authorRepository.GetFirstAsync(i => i.Id == id);
        if (result == null)
            throw new Exception("Author not found");

        return MapToDto(result);
    }

    public async Task<AuthorDto> UpdateAuthorAsync(Guid id, AuthorDto dto)
    {
        var resuult = await _authorRepository.GetFirstAsync(i => i.Id == id);
        if (resuult == null)
            throw new Exception("Author not found");

        resuult.Name = dto.Name;

        await _authorRepository.UpdateAsync(resuult);

        return MapToDto(resuult);
    }


    private AuthorDto MapToDto(Author author)
    {
        return new AuthorDto
        {
            Name = author.Name
        };
    }
}