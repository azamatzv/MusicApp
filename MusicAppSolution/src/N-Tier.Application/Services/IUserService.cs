using N_Tier.Application.DataTransferObjects;
using N_Tier.Core.DTOs;

namespace N_Tier.Application.Services;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(Guid id);
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> AddUserAsync(UserDto userDto);
    Task<UserDto> UpdateUserAsync(Guid id, UserDto userDto);
    Task<UserDto> AuthenticateAsync(LoginDto loginDto);
    Task<bool> DeleteUserAsync(Guid id);
}
