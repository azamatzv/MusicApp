using N_Tier.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.DataAccess.Repositories
{
    public interface IOtpRepository : IBaseRepository<OtpVerification>
    {
        Task<OtpVerification> GetActiveOtpAsync(Guid userId, string otpCode);
        Task<bool> HasActiveOtpAsync(Guid userId);
        Task DeleteExpiredOtpsAsync();
        Task InvalidateUserOtpsAsync(Guid userId);
    }
}
