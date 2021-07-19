using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistence;
using Microsoft.EntityFrameworkCore;
using API.Services;

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
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", corsoptions =>
                {
                    corsoptions.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            }
                );

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService,AccountService>();

            return services;
        }
    }
}
