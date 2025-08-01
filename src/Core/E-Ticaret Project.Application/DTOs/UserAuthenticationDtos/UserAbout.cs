﻿using E_Ticaret_Project.Application.DTOs.FavoriteDtos;
using E_Ticaret_Project.Application.DTOs.OrderDtos;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using E_Ticaret_Project.Domain.Enums;

namespace E_Ticaret_Project.Application.DTOs.UserAuthenticationDtos;

public record UserAbout(

    string Token,
    string Id,
    string FullName,
    string Email,
    string? ProfileImageUrl,
    RoleAdminEnum Role
);
