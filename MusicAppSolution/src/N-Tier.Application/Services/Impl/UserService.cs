using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Authentication;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> AddUserAsync(UserDto userDto)
    {


        string randomSalt = Guid.NewGuid().ToString();

        var user = new Users
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Password = _passwordHasher.Encrypt(
                password: userDto.Password,
                salt: randomSalt),
            CreatedBy = "System",
            Salt = randomSalt,
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