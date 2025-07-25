﻿using E_Ticaret_Project.Domain.Enums;

namespace E_Ticaret_Project.Application.DTOs.UserDtos;

public record UserGetDto(

    string Id,
    string FullName,
    string Email,
    string Role
);
