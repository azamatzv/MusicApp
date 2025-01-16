using FluentValidation;
using Microsoft.Extensions.Options;
using N_Tier.Application.DataTransferObjects;
using N_Tier.Application.DataTransferObjects.Authentication;
using N_Tier.Core.DTOs.UserDtos;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Authentication;
using N_Tier.DataAccess.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;

namespace N_Tier.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<UserDto> _userValidator;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<UpdateUserDto> _updateUserValidator;
    private readonly IOtpRepository _otpRepository;
    private readonly IJwtTokenHandler _jwtTokenHandler;

    public UserService(IUserRepository userRepository,
                       IPasswordHasher passwordHasher,
                       IValidator<UserDto> userValidator,
                       IValidator<LoginDto> loginValidator,
                       IValidator<UpdateUserDto> updateUserValidator,
                       IOtpRepository otpRepository,
                       IOptions<SmtpSettings> smtpSettings,
                       IJwtTokenHandler jwtTokenHandler)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _userValidator = userValidator;
        _loginValidator = loginValidator;
        _updateUserValidator = updateUserValidator;
        _otpRepository = otpRepository;
        _smtpSettings = smtpSettings.Value;
        _jwtTokenHandler = jwtTokenHandler;
    }

    private async Task SendOtpEmailAsync(string email, string otpCode)
    {
        try
        {
            var smtpClient = new SmtpClient(_smtpSettings.Server)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true,
            };

            var subject = "Email Verification OTP";
            var htmlContent = $"<strong>Your verification code is: {otpCode}. This code will expire in 10 minutes.</strong>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);

            Console.WriteLine("Email muvaffaqiyatli yuborildi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xatolik yuz berdi: {ex.Message}");
        }
    }


    private string GenerateOtpCode()
    {
        return new Random().Next(100000, 999999).ToString();
    }

    public async Task<string> ResendOtpAsync(Guid userId)
    {
        var user = await _userRepository.GetFirstAsync(u => u.Id == userId);
        if (user == null)
            throw new Exception("User not found");

        if (user.IsEmailVerified)
            throw new Exception("Email already verified");

        await _otpRepository.InvalidateUserOtpsAsync(userId);

        string newOtpCode = GenerateOtpCode();
        var newOtp = new OtpVerification
        {
            UserId = userId,
            OtpCode = newOtpCode,
            ExpirationDate = DateTime.Now.AddMinutes(10),
            IsUsed = false
        };

        await _otpRepository.AddAsync(newOtp);
        await SendOtpEmailAsync(user.Email, newOtpCode);

        return "New verification code has been sent to your email";
    }

    public async Task<(Guid userId, string message)> InitiateUserRegistrationAsync(UserDto userDto)
    {
        var validationResult = await _userValidator.ValidateAsync(userDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Validation failed", validationResult.Errors);
        }

        var existingUser = await _userRepository.GetFirstAsync(u => u.Email == userDto.Email);
        if (existingUser != null)
        {
            if (!existingUser.IsEmailVerified)
            {
                await _otpRepository.InvalidateUserOtpsAsync(existingUser.Id);
                string newOtpCode = GenerateOtpCode();
                var newOtp = new OtpVerification
                {
                    UserId = existingUser.Id,
                    OtpCode = newOtpCode,
                    ExpirationDate = DateTime.Now.AddMinutes(10),
                    IsUsed = false
                };

                await _otpRepository.AddAsync(newOtp);
                Console.WriteLine($@"Created new OTP:
UserId: {newOtp.UserId}
OtpCode: {newOtpCode}
ExpirationDate: {newOtp.ExpirationDate}
IsUsed: {newOtp.IsUsed}");
                await SendOtpEmailAsync(existingUser.Email, newOtpCode);

                return (existingUser.Id, "New verification code has been sent to your email");
            }

            throw new InvalidOperationException("Email already registered");
        }

        string salt = Guid.NewGuid().ToString();
        var user = new Users
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Password = _passwordHasher.Encrypt(userDto.Password, salt),
            Salt = salt,
            IsEmailVerified = false
        };

        var createdUser = await _userRepository.AddAsync(user);

        string otpCode = GenerateOtpCode();
        var otp = new OtpVerification
        {
            UserId = createdUser.Id,
            OtpCode = otpCode,
            ExpirationDate = DateTime.Now.AddMinutes(10),
            IsUsed = false
        };

        await _otpRepository.AddAsync(otp);
        await SendOtpEmailAsync(userDto.Email, otpCode);

        return (createdUser.Id, "Please check your email for verification code");
    }

    public async Task<(UserResponceDto user, string accessToken, string refreshToken)> VerifyAndCompleteRegistrationAsync(Guid userId, string otpCode, Guid tariffTypeId)
    {
        try
        {
            var otp = await _otpRepository.GetActiveOtpAsync(userId, otpCode);
            var user = await _userRepository.GetFirstAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            if (otp.IsUsed || otp.ExpirationDate < DateTime.Now)
            {
                if (otp.ExpirationDate < DateTime.Now)
                {
                    await _otpRepository.InvalidateUserOtpsAsync(userId);
                }
                throw new Exception("Invalid or expired OTP. Please request a new one.");
            }

            otp.IsUsed = true;
            await _otpRepository.UpdateAsync(otp);

            user.IsEmailVerified = true;
            await _userRepository.UpdateAsync(user);

            var account = new Accounts
            {
                UserId = user.Id,
                Name = user.Name,
                TariffTypeId = tariffTypeId
            };
            user.Accounts = new List<Accounts> { account };
            await _userRepository.UpdateAsync(user);

            var authUser = new AuthorizationUserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Password = user.Password
            };

            var jwtToken = await _jwtTokenHandler.GenerateAccessToken(authUser);
            var refreshToken = _jwtTokenHandler.GenerateRefreshToken();

            return (
                new UserResponceDto
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    TariffId = account.TariffTypeId
                },
                new JwtSecurityTokenHandler().WriteToken(jwtToken),
                refreshToken
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}, StackTrace: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<AuthorizationUserDto> AuthenticateAsync(LoginDto loginDto)
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

        return MapTodto(user);
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

    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto userDto)
    {
        var validationResult = await _updateUserValidator.ValidateAsync(userDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var user = await _userRepository.GetFirstAsync(u => u.Id == id);
        if (user == null) throw new Exception("User not found");

        if (!string.IsNullOrWhiteSpace(userDto.Name))
            user.Name = userDto.Name;
        if (!string.IsNullOrWhiteSpace(userDto.Email))
            user.Email = userDto.Email;

        if (!string.IsNullOrWhiteSpace(userDto.Password))
        {
            string newSalt = Guid.NewGuid().ToString();
            user.Password = _passwordHasher.Encrypt(password: userDto.Password, salt: newSalt);
            user.Salt = newSalt;
        }

        await _userRepository.UpdateAsync(user);

        return MapToDto(user);
    }



    private AuthorizationUserDto MapTodto(Users user)
    {
        return new AuthorizationUserDto
        {
            Id = user.Id,
            Email = user.Email,
            Password = user.Password,
            Role = user.Role
        };
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