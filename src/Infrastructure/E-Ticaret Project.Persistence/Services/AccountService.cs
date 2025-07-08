using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.UserDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Application.Shared.Settings;
using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Domain.Enums;
using E_Ticaret_Project.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace E_Ticaret_Project.Persistence.Services;

public class AccountService : IAccountService
{
    private UserManager<AppUser> _userManager { get; }
    private RoleManager<IdentityRole> _roleManager { get; }

    public AccountService(UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;

    }
    public async Task<BaseResponse<string>> AddRole(UserAddRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

        if (user is null)
            return new("User not found", HttpStatusCode.NotFound);

        var rolesNames = new List<string>();

        foreach (var roleId in dto.RolesId.Distinct())
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (role is null)
                return new($"Role not found: {roleId}", HttpStatusCode.NotFound);

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

        if (!users.Any())
            return new("Users not found", HttpStatusCode.NotFound);

        var allUsers = new List<UserGetDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault();

            allUsers.Add(new UserGetDto(
                Id: user.Id,
                FullName: user.FullName,
                Email: user.Email,
                Role:  roleName
                ));
        }
        return new("All users", allUsers, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<UserGetDto>> GetById(string userId)
    {
        var user=await _userManager.FindByIdAsync(userId);
        if(user is null)
            return new("User is not found",HttpStatusCode.NotFound);

        var roles=await _userManager.GetRolesAsync(user);
        var roleName = roles.FirstOrDefault();

        var newUser = new UserGetDto(
            Id:user.Id,
            FullName:user.FullName,
            Email: user.Email,
            Role:roleName
            );

        return new("User successfully founded",newUser, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> UserCreate(UserCreateDto dto)
    {
        var user=await _userManager.FindByEmailAsync(dto.Email);
        if(user is not null)
            return new("User already exist",HttpStatusCode.BadRequest);

        AppUser newUser = new()
        {
            FullName=dto.FullName,
            Email=dto.Email,
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

        newUser.EmailConfirmed = true;
        await _userManager.AddToRoleAsync(newUser, roleName);


        return new("Succesfully created", true, HttpStatusCode.Created);
    }
}
