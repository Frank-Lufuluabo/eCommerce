using eCommerce.Application.DTOs;
using FluentValidation;
using System.Security.Cryptography.X509Certificates;

namespace eCommerce.Application.Validations
{
    public interface IValidationService
    {
        Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator);
    }
}
