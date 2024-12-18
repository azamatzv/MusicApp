using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class CardsRepository : BaseRepository<Cards>, ICardsRepository
{
    public CardsRepository(DatabaseContext context) : base(context)
    {
    }
}
