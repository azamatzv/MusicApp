namespace N_Tier.Application.DataTransferObjects.Authentication;

public record TokenDto(string accessToken,
    string? refreshToken,
    DateTime expireDate);

