using N_Tier.Core.Common;

namespace N_Tier.Core.Entities;

public class Cards : BaseEntity, IAuditedEntity
{
    public Users User { get; set; }
    public Guid UserId { get; set; }

    public required long CardNumber { get; set; }

    public CardType CardType { get; set; }
    public Guid CardTypeId { get; set; }

    public required string Expire_Date { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

}
