namespace N_Tier.DataAccess.Authentication;

public interface IJwtTokenHandler
{
    JwtSecurityToken GenerateAccessToken(Users user);
    string GenerateRefreshToken();
}
