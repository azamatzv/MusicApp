using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;

namespace N_Tier.Application.Services;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(Guid id);
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> AddUserAsync(UserDto userDto);
    Task<UserDto> UpdateUserAsync(Guid id, UserDto userDto);

}
