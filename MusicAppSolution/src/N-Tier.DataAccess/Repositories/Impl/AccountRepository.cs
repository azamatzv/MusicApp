using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class AccountRepository : BaseRepository<Accounts>, IAccountsRepository
{
    public AccountRepository(DatabaseContext context) : base(context)
    {
    }
}
