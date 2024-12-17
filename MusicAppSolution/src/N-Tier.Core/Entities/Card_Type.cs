using N_Tier.Core.Common;

namespace N_Tier.Core.Entities;

public class Card_Type : BaseEntity, IAuditedEntity
{
    public required string Name { get; set; }

    public ICollection<Cards> Cards { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
