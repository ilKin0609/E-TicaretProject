namespace E_Ticaret_Project.Application.DTOs.InstaLinkDtos;

public record InstagramLinkVm
{
    public Guid Id { get; init; }
    public required string Permalink { get; init; } // canonical: https://www.instagram.com/p/XXXXXX/
    public required string EmbedUrl { get; init; }
}