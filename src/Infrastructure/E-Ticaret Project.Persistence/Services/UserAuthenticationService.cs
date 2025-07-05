using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using E_Ticaret_Project.Application.Shared;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Application.Shared.Settings;
using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using static E_Ticaret_Project.Application.Shared.Permissions.Permission;

namespace E_Ticaret_Project.Persistence.Services;

public class UserAuthenticationService : IUserAuthenticationService
{
    private SignInManager<AppUser> _signInManager { get; }
    private UserManager<AppUser> _userManager { get; }
    private IEmailService _mailService { get; }
    private RoleManager<IdentityRole> _roleManager { get; }
    private JWTSettings _jwtSetting { get; }
    private IFavoriteService _favoriteService { get; }

    public UserAuthenticationService(UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IOptions<JWTSettings> jwtSetting,
    RoleManager<IdentityRole> roleManager,
    IEmailService mailService,
    IFavoriteService favoriteService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSetting = jwtSetting.Value;
        _roleManager = roleManager;
        _mailService = mailService;
        _favoriteService = favoriteService;
    }
    public async Task<BaseResponse<string>> Register(UserRegisterDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is not null)
            return new("This user is already exist", HttpStatusCode.BadRequest);

        AppUser newUser = new()
        {

            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.Email
        };

        IdentityResult identityResult = await _userManager.CreateAsync(newUser, dto.Password);
        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors;
            StringBuilder errorsMessage = new();
            foreach (var error in errors)
            {
                errorsMessage.Append(error.Description + ";");
            }
            return new(errorsMessage.ToString(), HttpStatusCode.BadRequest);
        }

        var roleName = dto.Role.ToString();
        if (roleName is null)
            return new("Wrong format", HttpStatusCode.BadRequest);

        await _userManager.AddToRoleAsync(newUser, roleName);

        var confirmEmailLink = await GetEmailConfirmLink(newUser);

        await _mailService.SendEmailAsync(new List<string> { newUser.Email }, "Email Confirmation", confirmEmailLink);

        return new("Succesfully created", true, HttpStatusCode.Created);
    }
    public async Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto)
    {
        var existedUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existedUser is null)
        {
            return new("Email or password is wrong", HttpStatusCode.NotFound);
        }
        if (!existedUser.EmailConfirmed)
        {
            return new("Please confirm your email", HttpStatusCode.BadRequest);
        }
        SignInResult signInResult = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, true);
        if (!signInResult.Succeeded)
        {
            return new("Email or password is wrong", null, HttpStatusCode.NotFound);
        }

        var token = await GenerateTokenAsync(existedUser);
        return new("Token generated", token, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<UserAbout>> Me(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
            return new("Invalid token", HttpStatusCode.BadRequest);

        var jwtToken = handler.ReadJwtToken(token);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return new("User not found", HttpStatusCode.NotFound);

        var user = await _userManager.FindByIdAsync(userId);
        if(user is null)
            return new("User not found", HttpStatusCode.NotFound);

        var roles = await _userManager.GetRolesAsync(user);
        var roleName = roles.FirstOrDefault();

        //var orders = await _orderService.GetOrdersByUserId(user.Id);
        //var products = await _productService.GetProductsByUserId(user.Id);
        var favorites = await _favoriteService.MyFavorities(user.Id);

        var response = new UserAbout(
        Token: token,
        Id: user.Id,
        FullName: user.FullName,
        Email: user.Email,
        ProfileImageUrl: user.ProfileImageUrl,
        Role: Enum.Parse<RoleEnum>(roleName),
        Buyers: null,
        Sellers: null,
        Favorites: null
    );
       return new("Success",response,HttpStatusCode.OK); 

    }
    public async Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var pricipal = GetPrincipalFromExpiredToken(request.AccessToken);
        if (pricipal is null)
            return new("Invalid token", null, HttpStatusCode.NotFound);

        var userId = pricipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return new("User not found", null, HttpStatusCode.NotFound);

        if (user.RefreshToken is null || user.RefreshToken != request.RefreshToken || user.RefreshExpireDate < DateTime.UtcNow)
            return new("Invalid refresh token", null, HttpStatusCode.BadRequest);

        // Generate new tokens
        var tokenResponse = await GenerateTokenAsync(user);
        return new("Refreshed", tokenResponse, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> ConfirmEmail(string userId, string token)
    {
        var existedUser = await _userManager.FindByIdAsync(userId);
        if (existedUser is null)
        {
            return new("Email confirmation failed", HttpStatusCode.BadRequest);
        }
        var result = await _userManager.ConfirmEmailAsync(existedUser, token);

        if (!result.Succeeded)
        {
            return new("Email confirmation failed", HttpStatusCode.BadRequest);
        }
        return new("Email confirmed successfully", true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> ForgotPassword(UserForgotPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return new("User not existed", HttpStatusCode.NotFound);

        if (!user.EmailConfirmed)
            return new("Email is not confirmed", HttpStatusCode.BadRequest);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var link = $"https://localhost:7150/api/Authentications/ResetPassword?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";

        await _mailService.SendEmailAsync(new List<string> { user.Email }, "Reset password", link);

        return new("Link successfully sended.Please check your gmail", token, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> ResetPassword(UserResetPasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user is null)
            return new("User not existed", HttpStatusCode.NotFound);

        var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(x => x.Description));
            return new(errors, HttpStatusCode.BadRequest);
        }

        return new("Password succesfully changed.", true, HttpStatusCode.OK);
    }
    private async Task<string> GetEmailConfirmLink(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var link = $"https://localhost:7150/api/Authentications/ConfirmEmail?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";

        return link;
    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    private async Task<TokenResponse> GenerateTokenAsync(AppUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSetting.SecretKey);


        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };
        // 🔐 İstifadəçinin rollarını və onların permission claim-lərini əlavə et
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var roleName in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is not null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                var permissionClaims = roleClaims
                    .Where(c => c.Type == "Permission")
                    .Distinct();

                foreach (var permissionClaim in permissionClaims)
                {
                    claims.Add(permissionClaim); // ⬅️ Ən vacib yer budur!
                }
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSetting.ExpiryMinutes),
            Issuer = _jwtSetting.Issuer,
            Audience = _jwtSetting.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        var refreshToken = GenerateRefreshToken();

        var refreshTokenExpiryDate = DateTime.UtcNow.AddHours(2);

        user.RefreshToken = refreshToken;
        user.RefreshExpireDate = refreshTokenExpiryDate;

        await _userManager.UpdateAsync(user);

        return new TokenResponse
        {
            Token = jwt,
            RefreshToken = refreshToken,
            ExpireDate = tokenDescriptor.Expires!.Value
        };
    }
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false, //IMPORTANT: ignore expiration here
            ValidIssuer = _jwtSetting.Issuer,
            ValidAudience = _jwtSetting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecretKey))

        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var pricipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return pricipal;
            }
        }
        catch
        {
            return null;
        }
        return null;
    }
}
