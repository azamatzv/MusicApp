using N_Tier.Core.DTOs;

namespace N_Tier.Application.Services;

public interface IAuthorService
{
    Task<AuthorDto> GetByIdAsync(Guid id);
    Task<List<AuthorDto>> GetAllAsync();
    Task<AuthorDto> AddAuthorAsync(AuthorDto dto);
    Task<AuthorDto> UpdateAuthorAsync(Guid id, AuthorDto dto);
    Task<bool> DeleteAuthorAsync(Guid id);
}