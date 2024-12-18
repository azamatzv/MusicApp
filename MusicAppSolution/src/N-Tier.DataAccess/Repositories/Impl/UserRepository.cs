using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class UserRepository : BaseRepository<Users>, IUserRepository
{
    public UserRepository(DatabaseContext context) : base(context)
    {
    }
}
