namespace N_Tier.Application.DataTransferObjects;

public record TokenDto(string accessToken, string? refreshToken, DateTime expireDate);