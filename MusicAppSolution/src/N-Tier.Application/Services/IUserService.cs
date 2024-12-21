using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;

namespace N_Tier.Application.Services;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(int id);
    Task<List<UserDto>> GetAllAsync();
    Task<Users> AddUserAsync(UserDto userDto);
    Task<UserDto> UpdateUserAsync(int id, UserDto userDto);
    Task<bool> DeleteUserAsync(int id);
}
