﻿using N_Tier.Core.Entities;
using N_Tier.DataAccess.Persistence;

namespace N_Tier.DataAccess.Repositories.Impl;

public class TariffTypeRepository : BaseRepository<TariffType>, ITariffTypeRepository
{
    public TariffTypeRepository(DatabaseContext context) : base(context)
    {
    }
}
