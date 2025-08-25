using E_Ticaret_Project.Application.DTOs.InstaLinkDtos;
using E_Ticaret_Project.Domain.Entities;

public static class InstagramLinkMappers
{
    public static InstagramLink ToEntity(this InstaLinkCreateDto dto)
    {
        var canonical = InstagramUrlHelper.ToCanonicalPermalink(dto.Link)
                        ?? throw new ArgumentException("Instagram linki yanlışdır.");
        return new InstagramLink { Link = canonical };
    }

    public static InstagramLinkVm ToVm(this InstagramLink entity)
    {
        var permalink = InstagramUrlHelper.ToCanonicalPermalink(entity.Link) ?? entity.Link;
        return new InstagramLinkVm
        {
            Id = entity.Id,
            Permalink = permalink,
            EmbedUrl = InstagramEmbedHelper.ToEmbedUrl(permalink)
        };
    }
}