using Application.Interfaces;
using Application.Interfaces.Base;
using Application.Mappings;
using Application.Services;
using Application.Services.Base;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using Infra.Repositories;
using Infra.Repositories.Base;

namespace WebUI.Configuration;

public static class DependencyInjectionConfig
{
    public static void AddRegister(this IServiceCollection services, IConfiguration configuration)
    {
        #region Base
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IService<,>), typeof(Service<,>));
        #endregion

        #region Services
        // services.AddScoped<I[Entity]Service, [Entity]Service>();
        #endregion

        #region Repositories
        // services.AddScoped<I[Entity]Repositor, [Entity]Repository>();
        #endregion

        #region AutoMapper Profiles
        // services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
        #endregion
    }
}