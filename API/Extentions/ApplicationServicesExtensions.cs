using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistence;
using Microsoft.EntityFrameworkCore;


namespace API.Extentions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
                                                                 IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                });
            services.AddDbContext<DataContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
            );

            return services;
        }
    }
}
