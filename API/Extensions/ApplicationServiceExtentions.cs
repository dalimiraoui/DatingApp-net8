
using API.Data;
using API.Interfaces;
using API.Services;
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

        services.AddCors();
        return services;
    }

}
