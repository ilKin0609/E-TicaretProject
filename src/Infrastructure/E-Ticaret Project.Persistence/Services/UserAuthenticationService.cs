using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;
using E_Ticaret_Project.Application.Shared;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Application.Shared.Settings;
using E_Ticaret_Project.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace E_Ticaret_Project.Persistence.Services;

public class UserAuthenticationService : IUserAuthenticationService
{
    private SignInManager<AppUser> _signInManager { get; }
    private UserManager<AppUser> _userManager { get; }
    private IEmailService _mailService { get; }
    private RoleManager<IdentityRole> _roleManager { get; }
    private JWTSettings _jwtSetting { get; }

    private readonly string _receiverEmail;

    private IPasswordVault _vault { get; }

    private ILocalizationService _localizer { get; }
    private ISiteSettingRepository _siteRepo { get; }

    public UserAuthenticationService(UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IOptions<JWTSettings> jwtSetting,
    RoleManager<IdentityRole> roleManager,
    IEmailService mailService,
    IOptions<EmailSettings> emailOptions,
    ILocalizationService localizer,
    ISiteSettingRepository siteRepo,
    IPasswordVault vault)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSetting = jwtSetting.Value;
        _roleManager = roleManager;
        _mailService = mailService;
        _receiverEmail = emailOptions.Value.ReceiverEmail;
        _localizer = localizer;
        _siteRepo = siteRepo;
        _vault = vault;
    }
    public async Task<BaseResponse<string>> Register(UserRegisterDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is not null)
            return new(_localizer.Get("User_AlreadyExists"), HttpStatusCode.BadRequest);


        var subject = "Partnyor qeydiyyat sorğusu";
        var nl = Environment.NewLine;
        var body =
            $"Yeni partnyor sorğusu:{nl}" +
            $"Ad: {dto.Name}{nl}" +
            $"Soyad: {dto.Surname}{nl}" +
            $"Şirkət: {dto.Company}{nl}" +
            $"Vəzifə: {dto.Duty}{nl}" +
            $"Mobil: {dto.Phone}{nl}" +
            $"E-mail: {dto.Email}{nl}{nl}" +
            $"Sorğu göndərilmə vaxtı: {DateTime.Now}{nl}{nl}" +
            $"Zəhmət olmasa yoxlayın və qərar verin.";


        body = body.Replace("\n", "<br/>");

        var notify = await GetNotifyEmailAsync(); 
        await _mailService.SendEmailAsync(new List<string> { notify }, subject, body);


        return new(_localizer.Get("Partner_Register_Sent"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto)
    {
        var existedUser = await _userManager.FindByNameAsync(dto.UserName);
        if (existedUser is null)
        {
            return new(_localizer.Get("Auth_InvalidCredentials"), HttpStatusCode.NotFound);
        }
        if (existedUser.isToggle)
            return new(_localizer.Get("Login_Closed"), HttpStatusCode.BadRequest);

        if (!existedUser.EmailConfirmed)
        {
            return new(_localizer.Get("Auth_EmailNotConfirmed"), HttpStatusCode.BadRequest);
        }
        SignInResult signInResult = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, true, true);
        if (!signInResult.Succeeded)
        {
            return new(_localizer.Get("Auth_InvalidCredentials"), null, HttpStatusCode.NotFound);
        }
        existedUser.LastLoginAt = DateTime.Now;
        var token = await GenerateTokenAsync(existedUser);
        return new(_localizer.Get("Auth_TokenGenerated"), token, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> LogoutAsync(ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user is null)
            return new(_localizer.Get("Auth_User_NotFound"), HttpStatusCode.NotFound);

        // refresh tokeni etibarsız et
        user.RefreshToken = null;
        user.RefreshExpireDate = null;

        await _userManager.UpdateAsync(user);

        // əgər cookie ilə də login ola bilərsə, çıxışı et
        await _signInManager.SignOutAsync();

        return new(_localizer.Get("Auth_LoggedOut"), true, HttpStatusCode.OK);
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

    public async Task<BaseResponse<string>> ForgotPassword(UserForgotPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return new(_localizer.Get("Auth_User_NotExisted"), HttpStatusCode.NotFound);

        if (!user.EmailConfirmed)
            return new(_localizer.Get("Auth_EmailNotConfirmed"), HttpStatusCode.BadRequest);

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        var sentAtBaku = DateTimeOffset.UtcNow
    .ToOffset(TimeSpan.FromHours(4))
    .ToString("yyyy-MM-dd HH:mm:ss");

        if (!isAdmin)
        {
            var bodyForAdmin =
                $"Partnyordan parol dəyişdirmə sorğusu\n" +
                $"- Ad: {user.Name} {user.Surname}\n" +
                $"- Email: {user.Email}\n" +
                $"- Şirkət: {user.Company}\n" +
                $"- Vəzifə: {user.Duty}\n" +
                $"- Sorğu göndərilmə vaxtı : {sentAtBaku}";

            bodyForAdmin = bodyForAdmin.Replace("\n", "<br/>");

            var notify = await GetNotifyEmailAsync();   // <-- YENİ
            await _mailService.SendEmailAsync(new List<string> { notify },
                "Partnyor parolunun dəyişdirilməsi sorğusu", bodyForAdmin);

            return new(_localizer.Get("Partner_Reset_SentToAdmin"), true, HttpStatusCode.OK);
        }


        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var baseUrl = _jwtSetting.Audience; 
        var link = $"{baseUrl}/hesab/sifre-yenile/reset" +
                   $"?userId={user.Id}" +
                   $"&token={HttpUtility.UrlEncode(token)}";

        await _mailService.SendEmailAsync(new List<string> { user.Email! }, "Reset password", link);

        return new("Sıfırlama linki göndərildi. Zəhmət olmasa e-poçtunuzu yoxlayın.", token, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> ResetPassword(UserResetPasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user is null)
            return new("Istifadeci tapilmadi", HttpStatusCode.NotFound);
        var decodedToken = HttpUtility.UrlDecode(dto.Token);
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(x => x.Description));
            return new(errors, HttpStatusCode.BadRequest);
        }

        user.PasswordVault = _vault.Protect(dto.NewPassword);
        await _userManager.UpdateAsync(user);

        var body =
               $"Şifrə uğurla yeniləndi\n";

        body = body.Replace("\n", "<br/>");

        await _mailService.SendEmailAsync(new List<string> { user.Email! }, "Admin şifrə dəyişdirilməsi sorğusu", body);

        return new("Password succesfully changed.", true, HttpStatusCode.OK);
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
    private async Task<string> GetNotifyEmailAsync()
    {
        // DB-dən aktiv site setting-i oxu
        var dbEmail = await _siteRepo.GetAll()
            .Select(s => s.PublicEmail)
            .FirstOrDefaultAsync();

        // boşdursa appsettings-dəki ReceiverEmail-ə fallback
        return !string.IsNullOrWhiteSpace(dbEmail) ? dbEmail : _receiverEmail;
    }

}
