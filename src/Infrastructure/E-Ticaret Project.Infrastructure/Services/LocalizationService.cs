using E_Ticaret_Project.Application.Abstracts.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace E_Ticaret_Project.Infrastructure.Services;

public class LocalizationService:ILocalizationService
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Dictionary<string, Dictionary<string, string>> _localizations = new();

    public LocalizationService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        _env = env;
        _httpContextAccessor = httpContextAccessor;
        LoadLocalizationFiles();
    }

    private void LoadLocalizationFiles()
    {
        var path = Path.Combine(_env.ContentRootPath, "Localization");

        foreach (var file in Directory.GetFiles(path, "lang.*.json"))
        {
            var culture = Path.GetFileName(file).Split('.')[1]; // lang.az.json → az
            var json = File.ReadAllText(file);
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (data != null)
                _localizations[culture] = data;
        }
    }

    public string Get(string key)
    {
        var culture = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].FirstOrDefault() ?? "az";

        if (_localizations.TryGetValue(culture, out var dict) && dict.TryGetValue(key, out var value))
        {
            return value;
        }

        return key; // fallback if not found
    }
}
