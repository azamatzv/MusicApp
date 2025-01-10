using Microsoft.AspNetCore.Mvc;
using N_Tier.DataAccess.Authentication;
using System.Security.Claims;

namespace MusicApp.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID in token");
            }
            return userId;
        }

        protected string GetUserEmailFromToken()
        {
            var emailClaim = User.FindFirst(CustomClaimNames.Email)?.Value;
            if (string.IsNullOrEmpty(emailClaim))
            {
                throw new UnauthorizedAccessException("Invalid or missing email in token");
            }
            return emailClaim;
        }

        protected Guid GetAccountIdFromToken()
        {
            var accountIdClaim = User.FindFirst(CustomClaimNames.AccountId)?.Value;
            if (string.IsNullOrEmpty(accountIdClaim) || !Guid.TryParse(accountIdClaim, out Guid accountId))
            {
                throw new UnauthorizedAccessException("Invalid or missing account ID in token");
            }
            return accountId;
        }
    }
}
