﻿namespace E_Ticaret_Project.Application.Shared;

public class RefreshTokenRequest
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
