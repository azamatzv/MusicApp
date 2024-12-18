using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class FavouritesRepository : BaseRepository<Favourites>, IFavouritesRepository
{
    public FavouritesRepository(DatabaseContext context) : base(context)
    {
    }
}
