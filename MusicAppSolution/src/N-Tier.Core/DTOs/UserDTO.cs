using N_Tier.Core.Entities;
using System.ComponentModel;

namespace N_Tier.Core.DTOs;

public class UserDto
{
    [DefaultValue("")]
    public Guid TariffId { get; set; }
    [DefaultValue("")]
    public string? Name { get; set; }
    [DefaultValue("")]
    public string? Email { get; set; }
    [DefaultValue("")]
    public string? Password { get; set; }

    public Role Role { get; set; }
}
