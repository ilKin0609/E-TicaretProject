using System.Text.RegularExpressions;

public static class InstagramUrlHelper
{
    // Yalnız bu tipləri qəbul edirik: /p/{id}, /reel/{id}, /tv/{id}
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
        { "p", "reel", "tv" };

    // IG short-code üçün sadə yoxlama (rəqəm-hərf, _ və -)
    private static readonly Regex ShortCodeRx = new("^[A-Za-z0-9_-]+$", RegexOptions.Compiled);

    /// <summary>
    /// Gələn istənilən IG linkini canonical permalink-ə çevirir:
    /// https://www.instagram.com/{type}/{id}/
    /// Uğursuzdursa null.
    /// </summary>
    public static string? ToCanonicalPermalink(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        // Trim & boşluqları təmizlə
        raw = raw.Trim();

        if (!Uri.TryCreate(raw, UriKind.Absolute, out var uri))
            return null;

        // Yalnız instagram.com host-u (www., m. və s. daxil)
        var host = uri.Host.ToLowerInvariant();
        if (!host.EndsWith("instagram.com"))
            return null;

        // /{type}/{id} götür
        var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 2) return null;

        var type = segments[0];
        var id = segments[1];

        if (!Allowed.Contains(type)) return null;
        if (!ShortCodeRx.IsMatch(id)) return null;

        // Canonical, sorğu və fragmentləri atırıq
        return $"https://www.instagram.com/{type}/{id}/";
    }

    /// <summary>
    /// Uğurlu canonical varsa true qaytarır və out-a yazır.
    /// </summary>
    public static bool TryCanonicalPermalink(string? raw, out string canonical)
    {
        canonical = ToCanonicalPermalink(raw) ?? string.Empty;
        return !string.IsNullOrEmpty(canonical);
    }
}