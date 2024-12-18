using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class CardTypeRepository : BaseRepository<CardType>, ICardTypeRepository
{
    public CardTypeRepository(DatabaseContext context) : base(context)
    {
    }
}
