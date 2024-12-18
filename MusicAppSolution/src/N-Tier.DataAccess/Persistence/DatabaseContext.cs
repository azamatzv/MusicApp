
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using N_Tier.Core.Common;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Identity;
using N_Tier.Shared.Services;
using System.Reflection;

namespace N_Tier.DataAccess.Persistence;

public class DatabaseContext : IdentityDbContext<ApplicationUser>
{
    private readonly IClaimService _claimService;

    public DatabaseContext(DbContextOptions options, IClaimService claimService) : base(options)
    {
        _claimService = claimService;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }


    public DbSet<Accounts> Accounts { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cards> Cards { get; set; }
    public DbSet<CardType> CardTypes { get; set; }
    public DbSet<Downloads> Downloads { get; set; }
    public DbSet<Favourites> Favourites { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Music> Musics { get; set; }
    public DbSet<PaymentHistory> PaymentHistories { get; set; }
    public DbSet<TariffType> TariffTypes { get; set; }
    public DbSet<Users> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _claimService.GetUserId();
                    entry.Entity.CreatedOn = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _claimService.GetUserId();
                    entry.Entity.UpdatedOn = DateTime.Now;
                    break;
            }

        return await base.SaveChangesAsync(cancellationToken);
    }


}
