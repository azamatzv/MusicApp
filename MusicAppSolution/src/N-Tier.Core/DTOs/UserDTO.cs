namespace N_Tier.Core.DTOs;

public class UserDto
{
    public Guid TariffId { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }
}
