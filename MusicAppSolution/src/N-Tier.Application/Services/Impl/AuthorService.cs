using N_Tier.Core.DTOs.AuthorDtos;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Application.Services.Impl;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorDto> AddAuthorAsync(AuthorDto authorDto)
    {
        var author = new Author
        {
            Name = authorDto.Name
        };

        // Author entity'sini qo'shish
        var createdAuthor = await _authorRepository.AddAsync(author);

        return new AuthorDto
        {
            Name = createdAuthor.Name
        };
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
    {
        var authors = await _authorRepository.GetAllAsync(a => true);

        return authors.Select(author => new AuthorDto
        {
            Name = author.Name
        });
    }

    public async Task<AuthorDto> GetAuthorAsync(Guid id)
    {
        var author = await _authorRepository.GetFirstAsync(a => a.Id == id);

        return new AuthorDto
        {
            Name = author.Name
        };
    }
}
