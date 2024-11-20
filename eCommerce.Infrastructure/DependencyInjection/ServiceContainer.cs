using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;
using eCommerce.Infrastructure.Data;
using eCommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace eCommerce.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = "Default";
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString(connectionString),
            sqlOptions =>
            { // Ensure this is the correct assembly
                sqlOptions.MigrationsAssembly(typeof(ServiceContainer).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(); // Enable automatic retries for transient failures
            }),
         ServiceLifetime.Scoped);

            services.AddScoped<IGeneric<Product>, GenericRepository<Product>>();
            services.AddScoped<IGeneric<Category>, GenericRepository<Category>>();
            return services;
        }

    }
}
