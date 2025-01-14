using N_Tier.Core.Entities;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace N_Tier.Core.DTOs.UserDtos;

public class UserDto
{
    //[DefaultValue("")]
    //public Guid TariffId { get; set; }
    [DefaultValue("")]
    public string? Name { get; set; }
    [DefaultValue("")]
    public string? Email { get; set; }
    [DefaultValue("")]
    public string? Password { get; set; }
    [JsonIgnore]
    public Role Role { get; set; }
}
