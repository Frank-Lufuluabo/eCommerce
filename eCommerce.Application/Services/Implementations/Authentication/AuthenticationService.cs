using AutoMapper;
using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Identity;
using eCommerce.Application.Service.Interface.Authentication;
using eCommerce.Application.Service.Interface.Logging;
using eCommerce.Application.Validations;
using eCommerce.Domain.Entities.Identity;
using eCommerce.Domain.Interfaces.Authentication;
using FluentValidation;

namespace eCommerce.Application.Service.Implementation.Authentication
{
    public class AuthenticationService
        (ITokenManagement tokenManagement, IUserManagement userManagement, 
        IRoleManagement roleManagement, IAppLogger<AuthenticationService> logger,
        IMapper mapper, IValidator<CreateUser> createUserValidator,
        IValidator<LoginUser> loginUserValidator, IValidationService validationService): IAuthenticationService
    {
        public async Task<ServiceResponse> CreateUser(CreateUser user)
        {
            var _validationResult = await validationService.ValidateAsync(user, createUserValidator);
            if (!_validationResult.Success) return _validationResult;

            var mapperModel = mapper.Map<AppUser>(user);
            mapperModel.UserName = user.Email;
            mapperModel.PasswordHash = user.Password;

            var result = await userManagement.CreateUser(mapperModel);
            if (!result)
                return new ServiceResponse { Message = "Email Address might be already in use or  unknown error occurred" };

            var _user = await userManagement.GetUserByEmail(user.Email);
            var users = await userManagement.GetAllUser();
            bool assignedResult = await roleManagement.AddUserToRole(_user!, users!.Count() > 1 ? "User" : "Admin");

            if (!assignedResult)
            {
                // Remove user
                int removeUserResult = await userManagement.RemoveUserByEmail(user!.Email!);
                if (removeUserResult <= 0)
                {
                    // Error occured while rolling back changes
                    // then log the error
                    logger.LogError(
                        new Exception($"User witj Email as {_user.Email} failed to be remove as a result of role assigning issue"), 
                        "User could not be assigned Role");
                    return new ServiceResponse { Message = "Error occured in creating account" };
                }
            }
            return new ServiceResponse { Success = true, Message = "Account Created!" };

            // Verify Email 
        }

        public async Task<LoginResponse> LoginUser(LoginUser user)
        {
            var _validationResult = await validationService.ValidateAsync(user, loginUserValidator);
            if (!_validationResult.Success) 
                return new LoginResponse(Message: _validationResult.Message);

            var mapperModel = mapper.Map<AppUser>(user);
            mapperModel.PasswordHash = user.Password;

            bool loginResult = await userManagement.LoginUser(mapperModel);
            if (!loginResult)
                return new LoginResponse(Message: "Email not found or invalid credentials");

            var _user = await userManagement.GetUserById(user.Email);
            var claims = await userManagement.GetUserClaims(_user!.Email);

            string jwtToken = tokenManagement.GenerateToken(claims);
            string refreshToken = tokenManagement.GetRefreshToken();

            int saveTokenResult = 0;
            bool userTokenCheck = await tokenManagement.ValidateRefreshToken(refreshToken);
            if (userTokenCheck)
                saveTokenResult = await tokenManagement.UpdateRefreshToken(_user.Id, refreshToken);
            else
            saveTokenResult = await tokenManagement.AddRefreshToken(_user.Id, refreshToken);

            return saveTokenResult <= 0 ? new LoginResponse(Message: "Internal error occured while authenticating") :
                new LoginResponse(Success: true, Token: jwtToken, RefreshToken: refreshToken);
        }

        public async Task<LoginResponse> ReviveToken(string refreshToken)
        {
          bool validateTokenResult = await tokenManagement.ValidateRefreshToken(refreshToken);
            if (!validateTokenResult)
                return new LoginResponse(Message: "Invalid token");

            string userId = await tokenManagement.GetUserIdByRefreshToken(refreshToken);
            AppUser? user = await userManagement.GetUserById(userId);
            var claims = await userManagement.GetUserClaims(user!.Email!);
            string newJwtToken = tokenManagement.GenerateToken(claims);
            string newRefreshToken = tokenManagement.GetRefreshToken();
            await tokenManagement.UpdateRefreshToken(userId, newRefreshToken);
            return new LoginResponse(Success: true, Token: newJwtToken, RefreshToken: newRefreshToken);
        }
    }
}
