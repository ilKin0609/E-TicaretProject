﻿using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using E_Ticaret_Project.Application.DTOs.UserDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Application.Shared;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IUserAuthenticationService
{
    Task<BaseResponse<string>> Register(UserRegisterDto dto);
    Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto);
    Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<BaseResponse<string>> ConfirmEmail(string userId, string token);
    Task<BaseResponse<string>> ForgotPassword(UserForgotPasswordDto dto);
    Task<BaseResponse<string>> ResetPassword(UserResetPasswordDto dto);
    Task<BaseResponse<UserAbout>> Me(string Token);
}
