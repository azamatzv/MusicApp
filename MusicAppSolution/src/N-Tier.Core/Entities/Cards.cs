using N_Tier.Core.Common;

namespace N_Tier.Core.Entities;

public class Cards : BaseEntity, IAuditedEntity
{
    public Users User { get; set; }
    public Guid UserId { get; set; }

    public required int CardNumber { get; set; }

    public required string Expire_Date { get; set; }

    public CardType CardType { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}


public enum CardType
{
    Uzcard = 1,
    Humo = 2,
    Visa = 3,
    MasterCard = 4
}