using N_Tier.Core.Common;

namespace N_Tier.Core.Entities;

public class Genre : BaseEntity, IAuditedEntity
{
    public required string Name { get; set; }

    public ICollection<Music> Musics { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
