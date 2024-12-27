using N_Tier.Core.Common;

namespace N_Tier.Core.Entities;

public class Users : BaseEntity, IAuditedEntity
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public Role Role { get; set; } = Role.User;

    public string Salt { get; set; }

    public ICollection<Accounts> Accounts { get; set; }

    public ICollection<Cards> Cards { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

  
}



public enum Role
{
    User = 1,
    Admin = 2
}