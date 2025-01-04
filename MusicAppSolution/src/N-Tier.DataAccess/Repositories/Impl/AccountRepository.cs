using Microsoft.EntityFrameworkCore;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;
using System.Linq.Expressions;

namespace N_Tier.DataAccess.Repositories.Impl;

public class AccountRepository : BaseRepository<Accounts>, IAccountsRepository
{
    private readonly DatabaseContext context;

    public AccountRepository(DatabaseContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<Accounts> GetFirstAccountAsync(Expression<Func<Accounts, bool>> predicate)
    {
        return await context.Accounts
        .Where(a => !a.IsDeleted)
        .FirstOrDefaultAsync(predicate);
    }
}
