using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.AdminDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace E_Ticaret_Project.Persistence.Services;

public class AdminService : IAdminService
{
    private UserManager<AppUser> _userManager { get; }
    private RoleManager<IdentityRole> _roleManager { get; }
    private IPasswordVault _vault { get; }
    private ILocalizationService _localizer { get; }

    public AdminService(UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IPasswordVault vault,
    ILocalizationService localizer
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _vault = vault;
        _localizer = localizer;

    }
    public async Task<BaseResponse<string>> AddRole(UserAddRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

        if (user is null)
            return new(_localizer.Get("Auth_User_NotFound"), HttpStatusCode.NotFound);

        var rolesNames = new List<string>();

        foreach (var roleId in dto.RolesId.Distinct())
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (role is null)
                return new(_localizer.Get("Role_NotFound"), HttpStatusCode.NotFound);

            if (!await _userManager.IsInRoleAsync(user, role.Name!))
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name!);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return new($"Failed to add role '{role.Name}' to user: {errors}", HttpStatusCode.BadRequest);
                }
                rolesNames.Add(role.Name!);
            }
        }

        return new($"Successfully added roles:{string.Join(", ", rolesNames)} to user.", true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<UserGetDto>>> GetAll()
    {
        var users = await _userManager.Users.ToListAsync();
        if (!users.Any()) return new(_localizer.Get("Auth_User_NotFound"), HttpStatusCode.NotFound);

        var list = new List<UserGetDto>();
        foreach (var u in users)
        {
            var roles = await _userManager.GetRolesAsync(u);
            if (!roles.Contains("Partnyor")) continue;

            list.Add(new UserGetDto(
                userId: u.Id,
                FirstName: u.Name,
                LastName: u.Surname,
                Company: u.Company,
                Position: u.Duty,
                Phone: u.PhoneNumber,
                Email: u.Email,
                Login: u.UserName,
                Password: _vault.Unprotect(u.PasswordVault),
                RequestAt: u.PartnerRequestAt,
                LastLoginAt: u.LastLoginAt,
                isToggle: u.isToggle
            ));
        }

        return new(_localizer.Get("Users_All"), list, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> UserCreate(UserCreateDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is not null)
            return new(_localizer.Get("User_Email_AlreadyExists"), HttpStatusCode.BadRequest);

        AppUser newUser = new()
        {
            Name = dto.FirstName,
            Surname = dto.LastName,
            Email = dto.Email,
            Company = dto.Company,
            Duty = dto.Position,
            PhoneNumber = dto.Phone,
            UserName = dto.Login,
            PartnerRequestAt = dto.RequestAt
        };
        newUser.EmailConfirmed = true;

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

        newUser.PasswordVault = _vault.Protect(dto.Password);
        await _userManager.UpdateAsync(newUser);

        const string defaultRole = "Partnyor";

        // Rol yoxdursa yarat
        if (!await _roleManager.RoleExistsAsync(defaultRole))
        {
            var roleCreate = await _roleManager.CreateAsync(new IdentityRole(defaultRole));
            if (!roleCreate.Succeeded)
            {
                var err = string.Join("; ", roleCreate.Errors.Select(e => e.Description));
                return new($"User created, but role create failed: {err}", HttpStatusCode.PartialContent);
            }
        }


        if (!await _userManager.IsInRoleAsync(newUser, defaultRole))
        {
            var addRole = await _userManager.AddToRoleAsync(newUser, defaultRole);
            if (!addRole.Succeeded)
            {
                var err = string.Join("; ", addRole.Errors.Select(e => e.Description));
                return new($"User created, but adding to role failed: {err}", HttpStatusCode.PartialContent);
            }
        }

        return new(_localizer.Get("User_Created_Success"), true, HttpStatusCode.Created);
    }
    public async Task<BaseResponse<string>> UserDelete(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return new("Auth_User_NotFound", HttpStatusCode.NotFound);

        user.RefreshToken = null;
        user.RefreshExpireDate = null;
        await _userManager.UpdateAsync(user);

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return new(errors, HttpStatusCode.BadRequest);
        }

        return new("User_Delete_Success", true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> ToggleUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return new("Auth_User_NotFound", HttpStatusCode.NotFound);

        user.isToggle = true;

        await _userManager.UpdateAsync(user);

        return new(_localizer.Get("User_Account_Disabled"), true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> UnToggleUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return new("Auth_User_NotFound", HttpStatusCode.NotFound);

        user.isToggle = false;

        await _userManager.UpdateAsync(user);

        return new(_localizer.Get("User_Account_Enabled"), true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> UserUpdate(UserUpdateDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.userId);
        if (user is null)
            return new(_localizer.Get("Auth_User_NotFound"), HttpStatusCode.NotFound);

        // FirstName / LastName
        if (dto.FirstName is not null) user.Name = dto.FirstName.Trim();
        if (dto.LastName is not null) user.Surname = dto.LastName.Trim();

        // Company / Position
        if (dto.Company is not null) user.Company = dto.Company.Trim();
        if (dto.Position is not null) user.Duty = dto.Position.Trim();

        // Phone
        if (dto.Phone is not null) user.PhoneNumber = dto.Phone.Trim();

        // Email (unikallıq)
        if (dto.Email is not null && !string.Equals(dto.Email, user.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailOwner = await _userManager.FindByEmailAsync(dto.Email);
            if (emailOwner is not null && emailOwner.Id != user.Id)
                return new(_localizer.Get("User_Email_AlreadyExists"), HttpStatusCode.BadRequest);

            user.Email = dto.Email.Trim();
            user.EmailConfirmed = true;
        }

        // Login/UserName (unikallıq)
        if (dto.Login is not null && !string.Equals(dto.Login, user.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var loginOwner = await _userManager.FindByNameAsync(dto.Login);
            if (loginOwner is not null && loginOwner.Id != user.Id)
                return new(_localizer.Get("User_Login_AlreadyExists"), HttpStatusCode.BadRequest);

            user.UserName = dto.Login.Trim();
        }

        
        if (dto.RequestAt.HasValue)
        {
            var d = dto.RequestAt.Value;
            user.PartnerRequestAt = new DateOnly(d.Year, d.Month, d.Day);
        }

        // Password (admin köhnə şifrə bilmədən təyin edir → reset token ilə)
        if (dto.Password is not null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var pr = await _userManager.ResetPasswordAsync(user, token, dto.Password);
            if (!pr.Succeeded)
            {
                var err = string.Join("; ", pr.Errors.Select(e => e.Description));
                return new($"{_localizer.Get("User_Password_Update_Failed")}: {err}", HttpStatusCode.BadRequest);
            }
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var err = string.Join("; ", result.Errors.Select(e => e.Description));
            return new(err, HttpStatusCode.BadRequest);
        }

        return new(_localizer.Get("User_Updated"), true, HttpStatusCode.OK);
    }
}
