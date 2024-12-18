using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class MusicRepository : BaseRepository<Music>, IMusicRepository
{
    public MusicRepository(DatabaseContext context) : base(context)
    {
    }
}
