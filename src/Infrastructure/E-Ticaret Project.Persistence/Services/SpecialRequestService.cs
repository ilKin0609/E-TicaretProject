using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.SpecialRequestDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Application.Shared.Settings;
using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;

namespace E_Ticaret_Project.Persistence.Services;

public class SpecialRequestService : ISpecialRequestService
{
    private readonly ISpecialRequestRepository _specialRequestRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IEmailService _emailService;
    private readonly ILocalizationService _localizer;
    private readonly string _receiverEmail;
    private readonly IImageRepository _imageRepository;
    private readonly ISiteSettingRepository _siteRepo;

    public SpecialRequestService(
        ISpecialRequestRepository specialRequestRepository,
        ICloudinaryService cloudinaryService,
        IEmailService emailService,
        ILocalizationService localizer,
        IOptions<EmailSettings> emailOptions,
        IImageRepository imageRepository,
        ISiteSettingRepository siteRepo)
    {
        _specialRequestRepository = specialRequestRepository;
        _cloudinaryService = cloudinaryService;
        _emailService = emailService;
        _localizer = localizer;
        _receiverEmail = emailOptions.Value.ReceiverEmail;
        _imageRepository = imageRepository;
        _siteRepo = siteRepo;
    }

    public async Task<BaseResponse<string>> CreateRequest(SpecialRequestCreateDto dto)
    {

        var request = new SpecialRequest
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Phone = dto.Phone,
            Email = dto.Email,
            OrderAbout = dto.OrderAbout
        };

        if (dto.File is not null)
        {
            var (url, publicId) = await _cloudinaryService.UploadImageAsync(dto.File, "ecommerce/specialrequests");

            if (url is null)
                return new(_localizer.Get("File_UploadFail"), HttpStatusCode.BadRequest);

            var image = new Image
            {
                ImageUrl = url,
                PublicId = publicId
            };

            request.File = image;
        }

        await _specialRequestRepository.AddAsync(request);
        await _specialRequestRepository.SaveChangeAsync();



        var fileSection = "";
        if (request.File is not null)
        {
            var ext = Path.GetExtension(request.File.ImageUrl).ToLower();

            if (ext is ".jpg" or ".jpeg" or ".png")
            {
                fileSection = $@"
                    <p><b>{_localizer.Get("SpecialRequest_Email_AttachedFile")}:</b></p>
                    <a href='{request.File.ImageUrl}' target='_blank'>
                        <img src='{request.File.ImageUrl}' width='200' style='margin:5px' />
                    </a>";
            }
            else
            {
                fileSection = $@"
                    <p><b>{_localizer.Get("SpecialRequest_Email_AttachedFile")}:</b></p>
                    <p><a href='{request.File.ImageUrl}' target='_blank'>{Path.GetFileName(request.File.ImageUrl)}</a></p>";
            }
        }

        var encodedOrder = WebUtility.HtmlEncode(dto.OrderAbout);
        var emailBody = $@"
            <h2>{_localizer.Get("SpecialRequest_Email_Header")}</h2>
            <p><b>{_localizer.Get("SpecialRequest_Email_Name")}:</b> {dto.Name}</p>
            <p><b>{_localizer.Get("SpecialRequest_Email_Surname")}:</b> {dto.Surname}</p>
            <p><b>{_localizer.Get("SpecialRequest_Email_Phone")}:</b> {dto.Phone}</p>
            <p><b>{_localizer.Get("SpecialRequest_Email_Email")}:</b> {dto.Email}</p>
            <p><b>{_localizer.Get("SpecialRequest_Email_Message")}:</b><br/>{encodedOrder}</p>
            {fileSection}
        ";

        
        try
        {
            var notify = await GetNotifyEmailAsync();
            await _emailService.SendEmailAsync(
                new List<string> { notify },
                subject: _localizer.Get("SpecialRequest_Email_Subject"),
                body: emailBody
            );

            return new(_localizer.Get("SpecialRequest_Email_Success"),true, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Email göndərmə xətası: " + ex.Message);
            return new(_localizer.Get("SpecialRequest_Email_Failed"), HttpStatusCode.InternalServerError);
        }
    }
    public async Task<BaseResponse<string>> RemoveImageAsync(Guid imageId)
    {

        var image = await _imageRepository.GetByIdAsync(imageId);

        if (image is null || image.SpecialRequestId is null)
            return new(_localizer.Get("Image_NotFoundOrMismatch"), HttpStatusCode.NotFound);


        var deleteResult = await _cloudinaryService.DeleteImageAsync(image.PublicId);
        if (!deleteResult)
            return new(_localizer.Get("Image_CloudinaryDeleteFail"), HttpStatusCode.InternalServerError);


        _imageRepository.HardDelete(image);
        await _imageRepository.SaveChangeAsync();

        return new(_localizer.Get("Image_DeleteSuccess"), true, HttpStatusCode.OK);
    }



    private async Task<string> GetNotifyEmailAsync()
    {
        // DB-dən aktiv site setting-i oxu
        var dbEmail = await _siteRepo.GetAll()
            .Select(s => s.PublicEmail)
            .FirstOrDefaultAsync();

        // boşdursa appsettings-dəki ReceiverEmail-ə fallback
        return !string.IsNullOrWhiteSpace(dbEmail) ? dbEmail : _receiverEmail;
    }
}
