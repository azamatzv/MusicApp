using System.ComponentModel;

namespace N_Tier.Core.DTOs;

public class UpdateUserDto
{
    [DefaultValue("")]
    public string? Name { get; set; }

    [DefaultValue("")]
    public string? Email { get; set; }

    [DefaultValue("")]
    public string? Password { get; set; }
}
