﻿using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using E_Ticaret_Project.Application.Shared;
using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Ticaret_Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private IUserAuthenticationService _userAuthenticationService { get; }
        public AuthenticationsController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        // POST api/<AuthenticationsController>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _userAuthenticationService.Register(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var result = await _userAuthenticationService.Login(dto);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(BaseResponse<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
        {
            var result = await _userAuthenticationService.RefreshTokenAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> ForgotPassword([FromBody] UserForgotPasswordDto dto)
        {
            var result = await _userAuthenticationService.ForgotPassword(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "User.ResetPassword")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDto dto)
        {
            var result = await _userAuthenticationService.ResetPassword(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _userAuthenticationService.ConfirmEmail(userId, token);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Authorize(Policy = "User.GetMy")]
        [ProducesResponseType(typeof(BaseResponse<UserAbout>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> Me([FromQuery] string token)
        {
            var result = await _userAuthenticationService.Me(token);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
