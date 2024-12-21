using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Users> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetFirstAsync(u => u.Id == id);
        if (user == null) throw new Exception("User not found");
        return MapToDto(user);
    }

    public async Task<List<Users>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync(_ => true);
        return users.Select(MapToDto).ToList();
    }

    public async Task<Users> AddUserAsync(UserDto userDto)
    {
        var user = new Users
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Address = userDto.Address,
            PassportId = userDto.PassportId,
            CreatedBy = "System",
            Accounts = new List<Accounts>
        {
            new Accounts
            {
                Name = userDto.Name
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

    public async Task<Users> UpdateUserAsync(Guid id, UserDto userDto)
    {
        var user = await _userRepository.GetFirstAsync(u => u.Id == id);
        if (user == null) throw new Exception("User not found");

        user.Email = userDto.Email;
        user.Address = userDto.Address;
        user.PassportId = userDto.PassportId;

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

    private Users MapToDto(Users user)
    {
        return new Users
        {
            Name = user.Name,
            Email = user.Email,
            Address = user.Address,
            PassportId = user.PassportId
        };
    }

    public Task<UserDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateUserAsync(int id, UserDto userDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }
}
