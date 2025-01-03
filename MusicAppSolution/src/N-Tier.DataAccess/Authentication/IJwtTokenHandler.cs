﻿using N_Tier.Core.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace N_Tier.DataAccess.Authentication;

public interface IJwtTokenHandler
{
    JwtSecurityToken GenerateAccessToken(UserDto user);
    string GenerateRefreshToken();
}