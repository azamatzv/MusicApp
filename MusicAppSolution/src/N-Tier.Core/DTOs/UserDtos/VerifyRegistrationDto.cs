using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Core.DTOs.UserDtos;

public class VerifyRegistrationDto
{
    public Guid UserId { get; set; }
    public string OtpCode { get; set; }
    public Guid TariffTypeId { get; set; } = Guid.Empty;
}
