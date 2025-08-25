using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.InstaLinkDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using System.Net;

namespace E_Ticaret_Project.Persistence.Services;

public class InstaLinkService : IInstaLinkService
{
    private IInstaLinkRepository _instaRepo { get; }

    public InstaLinkService(IInstaLinkRepository instaRepo)
    {
        _instaRepo = instaRepo;
    }
    public async Task<BaseResponse<InstagramLinkVm>> AddAsync(InstaLinkCreateDto dto)
    {
        var canonical = InstagramUrlHelper.ToCanonicalPermalink(dto.Link);
        if (canonical is null)
            return new("Instagram linki yanlışdır (post/reel/tv).",HttpStatusCode.BadRequest);

        // Unikal təkrarı yoxla
        var exists = await _instaRepo.AnyAsync(x => x.Link == canonical);
        if (exists)
            return new("Bu post artıq mövcuddur.", HttpStatusCode.BadRequest);

        var entity = new InstagramLink { Link = canonical };

        await _instaRepo.AddAsync(entity);
        await _instaRepo.SaveChangeAsync();   // commit

        var result= new InstagramLinkVm
        {
            Id = entity.Id,
            Permalink = canonical,
            EmbedUrl = InstagramEmbedHelper.ToEmbedUrl(canonical)
        };
        return new("Link elave edildi", result,HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string>> DeleteAsync(InstagramLinkDeleteDto dto)
    {
        var canonical = InstagramUrlHelper.ToCanonicalPermalink(dto.Link);
        if (canonical is null)
            return new("Yanlış link formatı.",HttpStatusCode.BadRequest);

        // Tracking açıq (siləcəyik deyə)
        var entity =  _instaRepo
            .GetByIdFiltered(predicate: x => x.Link == canonical, isTracking: true)
            .FirstOrDefault();

        if (entity is null)
            return new("Link tapılmadı.",HttpStatusCode.NotFound);

        _instaRepo.HardDelete(entity);
        await _instaRepo.SaveChangeAsync();

        return new("Silindi.", HttpStatusCode.OK);
    }

    public async Task<IReadOnlyList<InstagramLinkVm>> GetRandomAsync(int count = 6)
    {
        if (count <= 0) count = 6;

        var list =  _instaRepo
            .GetAll(isTracking: false)
            .OrderBy(_ => Guid.NewGuid())   // ORDER BY NEWID() ekvivalenti
            .Take(count)
            .Select(x => new InstagramLinkVm
            {
                Id = x.Id,
                Permalink = x.Link,                          // DB-də artıq canonical
                EmbedUrl = InstagramEmbedHelper.ToEmbedUrl(x.Link)
            })
            .ToList();

        return list;
    }
}
