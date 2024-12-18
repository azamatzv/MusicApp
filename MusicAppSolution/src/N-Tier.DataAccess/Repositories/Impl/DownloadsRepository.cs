using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class DownloadsRepository : BaseRepository<Downloads>, IDownloadsRepository
{
    public DownloadsRepository(DatabaseContext context) : base(context)
    {
    }
}
