using N_Tier.Core.Common;

namespace N_Tier.Core.Entities;

public class Music : BaseEntity, IAuditedEntity
{
    public required string Name { get; set; }

    public Author Author { get; set; }
    public Guid AuthorId { get; set; }

    public Genre Genre { get; set; }
    public Guid GenreId { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
