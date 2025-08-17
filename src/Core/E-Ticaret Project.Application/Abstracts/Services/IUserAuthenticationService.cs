using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using E_Ticaret_Project.Application.Shared;
using E_Ticaret_Project.Application.Shared.Responses;
using System.Security.Claims;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IUserAuthenticationService
{
    Task<BaseResponse<string>> Register(UserRegisterDto dto);
    Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto);
    Task<BaseResponse<string>> LogoutAsync(ClaimsPrincipal principal);
    Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<BaseResponse<string>> ForgotPassword(UserForgotPasswordDto dto);
    Task<BaseResponse<string>> ResetPassword(UserResetPasswordDto dto);
    
}
