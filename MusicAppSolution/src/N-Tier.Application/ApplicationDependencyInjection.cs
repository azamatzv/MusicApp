using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using N_Tier.Application.Services;
using N_Tier.Application.Services.Impl;
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
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITariffTypeService, TariffTypeService>();
    }


    private static void RegisterCashing(this IServiceCollection services)
    {
        services.AddMemoryCache();
    }


}