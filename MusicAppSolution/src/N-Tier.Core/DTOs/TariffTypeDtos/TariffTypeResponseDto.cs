using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Core.DTOs.TariffTypeDtos;

public class TariffTypeResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
}
