using eCommerce.Application.Mapping;
using eCommerce.Application.Service.Implementation;
using eCommerce.Application.Service.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            return services;
        }
    }
}
