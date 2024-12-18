using N_Tier.Core.Common;

namespace N_Tier.Core.Entities;

public class PaymentHistory : BaseEntity, IAuditedEntity
{
    public Cards Cards { get; set; }
    public Guid CardsId { get; set; }

    public Accounts Accounts { get; set; }
    public Guid AccountsId { get; set; }

    public TariffType TariffType { get; set; }
    public Guid TarifId { get; set; }

    public bool IsPaid { get; set; } = true;

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
