using N_Tier.Core.Common;

namespace N_Tier.Core.Entities;

public class Favourites : BaseEntity, IAuditedEntity
{
    public Music Music { get; set; }
    public Guid MusicId { get; set; }

    public Accounts Accounts { get; set; }
    public Guid AccountsId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
