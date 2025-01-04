using N_Tier.Core.Entities;

namespace N_Tier.Core.DTOs;

public class AccountDto
{
    public string? Name { get; set; }
    public Guid TariffTypeId { get; set; }
    public int Balance { get; set; }
    public Guid UserId { get; set; }
}
