//using Microsoft.AspNet.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using N_Tier.DataAccess.Identity;

//namespace N_Tier.DataAccess.Persistence;

//public class AutomatedMigration
//{
//    public static async Task MigrateAsync(IServiceProvider services)
//    {
//        var context = services.GetRequiredService<DatabaseContext>();

//        if (context.Database.IsNpgsql()) await context.Database.MigrateAsync();

//        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

//        await DatabaseContextSeed.SeedDatabaseAsync(context, userManager);
//    }
//}
