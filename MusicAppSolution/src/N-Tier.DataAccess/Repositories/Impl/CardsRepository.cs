using Microsoft.EntityFrameworkCore;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;
using System.Linq.Expressions;

namespace N_Tier.DataAccess.Repositories.Impl;

public class CardsRepository : BaseRepository<Cards>, ICardsRepository
{
    private readonly DatabaseContext context;
    public CardsRepository(DatabaseContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<Cards> GetFirstExistCardAsync(Expression<Func<Cards, bool>> predicate)
    {
        return await context.Cards
        .FirstOrDefaultAsync(predicate);
    }
}
