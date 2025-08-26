using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.AboutUsDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_Ticaret_Project.Persistence.Services;

public class AboutService : IAboutService
{
    private IAboutRepository _aboutRepository { get; }
    private ICloudinaryService _cloudinaryService { get; }
    private IImageRepository _imageRepository { get; }
    private IValidator<AboutUsUploadImageDto> _createValidator { get; }
    private IValidator<AboutUsUploadImageDto> _updateValidator { get; }

    private readonly ILocalizationService _localizer;
    public AboutService(IAboutRepository aboutRepository,
        IImageRepository imageRepository,
        ICloudinaryService cloudinaryService,
        ILocalizationService localizer,
        IValidator<AboutUsUploadImageDto> createValidator,
        IValidator<AboutUsUploadImageDto> updateValidator)
    {
        _aboutRepository = aboutRepository;
        _cloudinaryService = cloudinaryService;
        _imageRepository = imageRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _localizer = localizer;
    }
    public async Task<BaseResponse<AboutUsGetDto>> GetAbout()
    {
        var about = _aboutRepository.GetByIdFiltered(
            include: [x => x.Image]
        ).FirstOrDefault();


        if (about is null)
            return new(_localizer.Get("About_NotFound"), HttpStatusCode.NotFound);

        var NewDto = new AboutUsGetDto(
           MetaTitle_Az: about.MetaTitle_Az,
           MetaTitle_En: about.MetaTitle_En,
           MetaTitle_Ru: about.MetaTitle_Ru,

           MetaDescription_Az: about.MetaDescription_Az,
           MetaDescription_En: about.MetaDescription_En,
           MetaDescription_Ru: about.MetaDescription_Ru,

           Keywords: about.Keywords,

           TitleAZ: about.TitleAZ,
           TitleEN: about.TitleEN,
           TitleRU: about.TitleRU,

           DescriptionAZ: about.DescriptionAZ,
           DescriptionEN: about.DescriptionEN,
           DescriptionRU: about.DescriptionRU,

           ImageUrl: about.Image?.ImageUrl
       );

        return new(_localizer.Get("About_Found"), NewDto, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> UpdateAsync(AboutUsUpdateDto dto)
    {
        var about = _aboutRepository.GetByIdFiltered(
           include: [x => x.Image]
       ).FirstOrDefault();

        if (about is null)
            return new(_localizer.Get("About_NotFound"), HttpStatusCode.NotFound);

        if (!string.IsNullOrWhiteSpace(dto.MetaTitle_Az))
            about.MetaTitle_Az = dto.MetaTitle_Az;

        if (!string.IsNullOrWhiteSpace(dto.MetaTitle_En))
            about.MetaTitle_En = dto.MetaTitle_En;

        if (!string.IsNullOrWhiteSpace(dto.MetaTitle_Ru))
            about.MetaTitle_Ru = dto.MetaTitle_Ru;

        // MetaDescription
        if (!string.IsNullOrWhiteSpace(dto.MetaDescription_Az))
            about.MetaDescription_Az = dto.MetaDescription_Az;

        if (!string.IsNullOrWhiteSpace(dto.MetaDescription_En))
            about.MetaDescription_En = dto.MetaDescription_En;

        if (!string.IsNullOrWhiteSpace(dto.MetaDescription_Ru))
            about.MetaDescription_Ru = dto.MetaDescription_Ru;

        // Keywords
        if (!string.IsNullOrWhiteSpace(dto.Keywords))
            about.Keywords = dto.Keywords;

        // Titles
        if (!string.IsNullOrWhiteSpace(dto.TitleAZ))
            about.TitleAZ = dto.TitleAZ;

        if (!string.IsNullOrWhiteSpace(dto.TitleEN))
            about.TitleEN = dto.TitleEN;

        if (!string.IsNullOrWhiteSpace(dto.TitleRU))
            about.TitleRU = dto.TitleRU;

        // Descriptions
        if (!string.IsNullOrWhiteSpace(dto.DescriptionAZ))
            about.DescriptionAZ = dto.DescriptionAZ;

        if (!string.IsNullOrWhiteSpace(dto.DescriptionEN))
            about.DescriptionEN = dto.DescriptionEN;

        if (!string.IsNullOrWhiteSpace(dto.DescriptionRU))
            about.DescriptionRU = dto.DescriptionRU;


        _aboutRepository.Update(about);
        await _aboutRepository.SaveChangeAsync();

        return new(_localizer.Get("About_Updated"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> AboutUploadImage(AboutUsUploadImageDto dto)
    {
        var about = _aboutRepository.GetByIdFiltered(include: [x => x.Image]).FirstOrDefault();

        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new BaseResponse<string>(message, HttpStatusCode.BadRequest);
        }

        var (imageUrl, publicId) = await _cloudinaryService.UploadImageAsync(dto.ImageFile, "ecommerce/about");

        if (imageUrl == null)
            return new(_localizer.Get("Image_UploadFailed"), HttpStatusCode.BadRequest);

        var newImage = new Image
        {
            ImageUrl = imageUrl,
            PublicId = publicId,
            AboutUsId = about.Id,
            IsMain = true
        };
        about.Image = newImage;
        await _imageRepository.AddAsync(newImage);
        await _imageRepository.SaveChangeAsync();

        await _aboutRepository.SaveChangeAsync();

        return new(_localizer.Get("Image_UploadSuccess"), true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> RemoveImageAsync()
    {

        var image = await _imageRepository.GetAllFiltered(i => i.AboutUsId != null)
                                      .FirstOrDefaultAsync();

        if (image is null || image.AboutUsId is null)
            return new(_localizer.Get("Image_NotFoundOrMismatch"), HttpStatusCode.NotFound);

       
        var deleteResult = await _cloudinaryService.DeleteImageAsync(image.PublicId);
        if (!deleteResult)
            return new(_localizer.Get("Image_CloudinaryDeleteFail"), HttpStatusCode.InternalServerError);

  
        _imageRepository.HardDelete(image);
        await _imageRepository.SaveChangeAsync();

        return new(_localizer.Get("Image_DeleteSuccess"), true, HttpStatusCode.OK);
    }










    private static string? TryExtractPublicIdFromCloudinaryUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return null;

        var parts = uri.AbsolutePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);

        // ".../upload/[transformasiya].../v123/<public_id>.<ext>"
        var uploadIdx = Array.FindIndex(parts, p => p.Equals("upload", StringComparison.OrdinalIgnoreCase));
        if (uploadIdx < 0) return null;

        // 'v123' olan hissəni tap və ondan sonranı götür
        var vIdx = Array.FindIndex(parts, uploadIdx + 1, p => p.Length > 1 && p[0] == 'v' && int.TryParse(p.AsSpan(1), out _));
        if (vIdx < 0 || vIdx + 1 >= parts.Length) return null;

        var withExt = string.Join('/', parts[(vIdx + 1)..]);
        // uzantını at (jpg/png və s.)
        return System.IO.Path.ChangeExtension(withExt, null);
    }
}
