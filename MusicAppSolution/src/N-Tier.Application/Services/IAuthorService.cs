using N_Tier.Core.DTOs.AuthorDtos;
using N_Tier.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Application.Services;

public interface IAuthorService
{
    Task<AuthorDto> AddAuthorAsync(AuthorDto authorDto);
    Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
    Task<AuthorDto> GetAuthorAsync(Guid id);
}
