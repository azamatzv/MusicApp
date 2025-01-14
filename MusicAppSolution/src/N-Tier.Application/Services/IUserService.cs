using N_Tier.Application.DataTransferObjects.Authentication;
using N_Tier.Core.DTOs.UserDtos;
using N_Tier.Core.Entities;

namespace N_Tier.Application.Services;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(Guid id);
    Task<List<UserDto>> GetAllAsync();
    // Task<UserResponceDto> AddUserAsync(UserDto userDto);
    Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
    Task<AuthorizationUserDto> AuthenticateAsync(LoginDto loginDto);
    Task<(Guid userId, string message)> InitiateUserRegistrationAsync(UserDto userDto);
    Task<UserResponceDto> VerifyAndCompleteRegistrationAsync(Guid userId, string otpCode, Guid tariffTypeId);
}
