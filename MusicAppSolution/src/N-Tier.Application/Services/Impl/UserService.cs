using FluentValidation;
using N_Tier.Application.DataTransferObjects.Authentication;
using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Authentication;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<UserDto> _userValidator;
    private readonly IValidator<LoginDto> _loginValidator;

    public UserService(IUserRepository userRepository,
                       IPasswordHasher passwordHasher,
                       IValidator<UserDto> userValidator,
                       IValidator<LoginDto> loginValidator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _userValidator = userValidator;
        _loginValidator = loginValidator;
    }

    public async Task<UserDto> AddUserAsync(UserDto userDto)
    {
        try
        {
            var validationResult = await _userValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }


            string salt = Guid.NewGuid().ToString();
            var user = new Users
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = _passwordHasher.Encrypt(password: userDto.Password, salt: salt),
                Salt = salt,
                Accounts = new List<Accounts>
            {
                new Accounts
                {
                    Name = userDto.Name,
                    TariffTypeId = userDto.TariffId
                }
            }
            };

            var createdUser = await _userRepository.AddAsync(user);

            var account = createdUser.Accounts.FirstOrDefault();
            if (account != null)
            {
                account.UserId = createdUser.Id;
                account.Name = createdUser.Name;
                await _userRepository.UpdateAsync(createdUser);
            }

            return MapToDto(createdUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message, "StackTrace: ", ex.StackTrace + "InnerException: " + ex.InnerException);
            throw;
        }
    }

    public async Task<UserDto> AuthenticateAsync(LoginDto loginDto)
    {
        var validationResult = await _loginValidator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await _userRepository.GetFirstAsync(u => u.Email == loginDto.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var isPasswordValid = _passwordHasher.Verify(user.Password, loginDto.Password, user.Salt);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        return MapToDto(user);
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