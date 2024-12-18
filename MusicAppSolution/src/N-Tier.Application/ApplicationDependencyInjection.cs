using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N_Tier.Shared.Services;
using N_Tier.Shared.Services.Impl;

namespace N_Tier.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddServices(env);



        services.RegisterCashing();

        return services;
    }

    private static void AddServices(this IServiceCollection services, IWebHostEnvironment env)
    {

        services.AddScoped<IClaimService, ClaimService>();
    }



    private static void RegisterCashing(this IServiceCollection services)
    {
        services.AddMemoryCache();
    }




}
