using N_Tier.Core.Common;

namespace N_Tier.Core.Entities;

public class Users : BaseEntity, IAuditedEntity
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Address { get; set; }

    public required string PassportId { get; set; }

    public ICollection<Accounts> Accounts { get; set; }

    public ICollection<Cards> Cards { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
