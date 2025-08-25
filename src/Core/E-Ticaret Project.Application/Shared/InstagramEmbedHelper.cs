public static class InstagramEmbedHelper
{
    public static string ToEmbedUrl(string canonicalPermalink)
    {
        if (!canonicalPermalink.EndsWith("/")) canonicalPermalink += "/";
        return $"{canonicalPermalink}embed/";
    }
}