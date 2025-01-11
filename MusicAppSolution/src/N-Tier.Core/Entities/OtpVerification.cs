using N_Tier.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Core.Entities;

public class OtpVerification : BaseEntity
{
    public Users User { get; set; }
    public Guid UserId { get; set; }
    public string OtpCode { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsUsed { get; set; }

}
