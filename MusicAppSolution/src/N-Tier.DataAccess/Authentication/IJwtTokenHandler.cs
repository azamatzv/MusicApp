using N_Tier.Core.DTOs.UserDtos;
using N_Tier.Core.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace N_Tier.DataAccess.Authentication;

public interface IJwtTokenHandler
{
    Task<JwtSecurityToken> GenerateAccessToken(AuthorizationUserDto user);
    string GenerateRefreshToken();
}
