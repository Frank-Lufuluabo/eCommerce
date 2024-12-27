using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Identity;
using eCommerce.Application.Service.Interface.Authentication;
using eCommerce.Application.Service.Interface.Logging;
using eCommerce.Domain.Interfaces.Authentication;

namespace eCommerce.Application.Service.Implementation.Authentication
{

    public class AuthenticationService
        (ITokenManagement tokenManagement, IUserManagement userManagement, 
        IRoleManagement roleManagement, IAppLogger<AuthenticationService> logger): IAuthenticationService
    {
        public Task<ServiceResponse> CreateUser(CreateUser user)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> LoginUser(LoginUser user)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> ReviveToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
