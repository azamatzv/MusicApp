using N_Tier.Core.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace N_Tier.DataAccess.Authentication;

public interface IJwtTokenHandler
{
    JwtSecurityToken GenerateAccessToken(Users user);
    string GenerateRefreshToken();
}
