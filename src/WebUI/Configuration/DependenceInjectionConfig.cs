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
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion

        #region Services
        services.AddScoped<IAlbumService, AlbumService>();
        services.AddScoped<IArtistService, ArtistService>();
        services.AddScoped<IMusicService, MusicService>();
        services.AddScoped<IPlaylistService, PlaylistService>();
        #endregion

        #region Repositories
        services.AddScoped<IAlbumRepository, AlbumRepository>();
        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<IMusicRepository, MusicRepository>();
        services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        #endregion

        #region AutoMapper
        services.AddAutoMapper(typeof(EntityMappingProfile).Assembly);
        #endregion
    }
}