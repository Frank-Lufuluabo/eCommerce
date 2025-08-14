using eCommerce.Application.Mapping;
using eCommerce.Application.Service.Implementation;
using eCommerce.Application.Service.Interface;
using eCommerce.Application.Validations.Authentication;
using eCommerce.Application.Validations;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using eCommerce.Application.Service.Interface.Authentication;
using eCommerce.Application.Service.Implementation.Authentication;

namespace eCommerce.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
