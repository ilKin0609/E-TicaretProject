﻿namespace E_Ticaret_Project.Application.Shared.Responses;

public class TokenResponse
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime ExpireDate { get; set; }
}
