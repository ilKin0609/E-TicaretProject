using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace E_Ticaret_Project.Application.Shared;

public static class CurrentUserHelper
{
    public static string? GetUserId(HttpContext httpContext)
    {
        return httpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static string? GetUsername(HttpContext httpContext)
    {
        return httpContext?.User?.FindFirstValue(ClaimTypes.Name);
    }

    public static string? GetEmail(HttpContext httpContext)
    {
        return httpContext?.User?.FindFirstValue(ClaimTypes.Email);
    }

    // Əgər tokenə özəl (custom) claim-lər əlavə etmisənsə:
    public static string? GetClaim(HttpContext httpContext, string claimType)
    {
        return httpContext?.User?.FindFirst(claimType)?.Value;
    }
}
