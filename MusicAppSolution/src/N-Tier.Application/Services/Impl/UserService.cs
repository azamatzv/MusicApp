using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> AddUserAsync(UserDto userDto)
    {
        var user = new Users
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Password = userDto.Password,
            CreatedBy = "System",
            Accounts = new List<Accounts>
        {
            new Accounts
            {
                Name = userDto.Name,
                TariffTypeId=userDto.TariffId

            }
        }
        };


        var createdUser = await _userRepository.AddAsync(user);

        var account = createdUser.Accounts.FirstOrDefault();
        if (account != null)
        {
            account.UserId = createdUser.Id;
            account.Name = createdUser.Name;
        }

        await _userRepository.UpdateAsync(createdUser);

        return MapToDto(createdUser);
    }


    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetFirstAsync(u => u.Id == id);
        if (user == null) throw new Exception("User not found");
        return MapToDto(user);
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync(_ => true);
        return users.Select(MapToDto).ToList();
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UserDto userDto)
    {
        var user = await _userRepository.GetFirstAsync(u => u.Id == id);
        if (user == null) throw new Exception("User not found");

        user.Email = userDto.Email;
        user.Password = userDto.Password;

        await _userRepository.UpdateAsync(user);

        return MapToDto(user);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetFirstAsync(u => u.Id == id);
        if (user == null) throw new Exception("User not found");

        await _userRepository.DeleteAsync(user);
        return true;
    }


    private UserDto MapToDto(Users user)
    {
        return new UserDto
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password
        };
    }
}