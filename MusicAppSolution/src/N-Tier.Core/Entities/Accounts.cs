using N_Tier.Core.Common;

namespace N_Tier.Core.Entities
{
    public class Accounts : BaseEntity, IAuditedEntity
    {
        public required string Name { get; set; }

        public TariffType TariffType { get; set; }
        public Guid TariffTypeId { get; set; } = Guid.Empty;

        public int Balance { get; set; } = 0;

        public Users User { get; set; }
        public Guid UserId { get; set; } = Guid.Empty;

        public bool IsDeleted { get; set; } = false;

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
