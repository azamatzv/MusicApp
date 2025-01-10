using System.ComponentModel;

namespace N_Tier.Core.DTOs.AccountDto;

public class UpdateAccountDto
{
    [DefaultValue("")]
    public string? Name { get; set; }

    [DefaultValue(null)]
    public Guid? TariffTypeId { get; set; }
}
