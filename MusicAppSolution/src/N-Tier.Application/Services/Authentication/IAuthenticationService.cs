using N_Tier.Application.DataTransferObjects.Authentication;

namespace N_Tier.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<TokenDto> LoginAsync(AuthenticationDto authenticationDto);
    Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
}
