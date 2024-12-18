using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class GenreRepository : BaseRepository<Genre>, IGenreRepository
{
    public GenreRepository(DatabaseContext context) : base(context)
    {
    }
}
