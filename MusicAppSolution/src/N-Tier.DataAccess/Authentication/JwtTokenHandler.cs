using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using N_Tier.Core.DTOs.UserDtos;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.DataAccess.Authentication;

public class JwtTokenHandler : IJwtTokenHandler
{
    private readonly JwtOption jwtOption;
    private readonly IAccountsRepository _accountsRepository;

    public JwtTokenHandler(IOptions<JwtOption> options, IAccountsRepository accountsRepository)
    {
        _accountsRepository = accountsRepository;
        this.jwtOption = options.Value;
    }



    public async Task<JwtSecurityToken> GenerateAccessToken(AuthorizationUserDto user)
    {
        if (string.IsNullOrWhiteSpace(jwtOption.SecretKey) ||
    string.IsNullOrWhiteSpace(jwtOption.Issuer) ||
    string.IsNullOrWhiteSpace(jwtOption.Audience) ||
    jwtOption.ExpirationInMinutes <= 0)
        {
            throw new ArgumentException("Invalid JWT configuration. Ensure all required fields are set.");
        }

        if (user == null)
            throw new ArgumentNullException(nameof(user), "User cannot be null.");



        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("User email cannot be null or empty.", nameof(user.Email));

        if (string.IsNullOrWhiteSpace(jwtOption.SecretKey))
            throw new ArgumentNullException(nameof(jwtOption.SecretKey), "SecretKey cannot be null or empty.");

        var account = await _accountsRepository.GetFirstAsync(a => a.UserId == user.Id);
        if (account == null)
            throw new Exception("No account found for user");

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(CustomClaimNames.Email, user.Email),
        new Claim(CustomClaimNames.AccountId, account.Id.ToString()),
        //new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim("Role", user.Role.ToString())
    };

        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOption.SecretKey));

        var token = new JwtSecurityToken(
            issuer: jwtOption.Issuer,
            audience: jwtOption.Audience,
            expires: DateTime.UtcNow.AddMinutes(jwtOption.ExpirationInMinutes),
            claims: claims,
            signingCredentials: new SigningCredentials(
                key: authSigningKey,
                algorithm: SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    public string GenerateRefreshToken()
    {
        byte[] bytes = new byte[64];

        using var randomGenerator =
            RandomNumberGenerator.Create();

        randomGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
