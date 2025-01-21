
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtentions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();

        services.AddDbContext<DataContext>( opt=>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        // add services life Cycle
        services.AddScoped<ITokenService,TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILikesRepository, LikesRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<LogUserActivity>();
        // Registers AutoMapper with the application's service container.
        // Scans the current application's assemblies for mapping profiles 
        // (classes that inherit from AutoMapper.Profile) to configure object-to-object mappings automatically.
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        // Configures the CloudinarySettings object by binding its properties to the "cloudinarySetting" section in the appsettings.json configuration file
        services.Configure<CloudinarySetting>(config.GetSection("cloudinarySetting"));

        services.AddSignalR();

        services.AddSingleton<PresenceTracker>();


        services.AddCors();
        return services;
    }

}
