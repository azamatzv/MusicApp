using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;
using System.Linq.Expressions;

namespace N_Tier.DataAccess.Repositories;

public interface ICardsRepository : IBaseRepository<Cards>
{
    Task<Cards> GetFirstExistCardAsync(Expression<Func<Cards, bool>> predicate);
}
