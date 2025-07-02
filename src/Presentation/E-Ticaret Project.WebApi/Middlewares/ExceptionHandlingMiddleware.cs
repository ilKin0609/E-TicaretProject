using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace E_Ticaret_Project.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Sonraki middleware kec
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occured");


            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new BaseResponse<string>(
                message: "An unexpected error occured. Please try again later.",
                isSuccess: false,
                statusCode: HttpStatusCode.InternalServerError
            );

            var result = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await context.Response.WriteAsync(result);
        }
    }
}
