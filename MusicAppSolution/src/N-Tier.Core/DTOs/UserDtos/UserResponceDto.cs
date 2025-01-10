using N_Tier.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Core.DTOs.UserDtos;

public class UserResponceDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string Role { get; set; }
    public Guid TariffId { get; set; }
}
