using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ContactInfoDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using System.Net;

namespace E_Ticaret_Project.Persistence.Services;

public class ContactInfoService : IContactInfoService
{
    private IContactInfoRepository _contactRepository { get; }
    private ILocalizationService _localizer { get; }
    public ContactInfoService(IContactInfoRepository contactRepository
        , ILocalizationService localizer)
    {
        _contactRepository = contactRepository;
        _localizer = localizer;
    }
    public async Task<BaseResponse<ContactInfoGetDto>> GetContact()
    {
        var contact = _contactRepository.GetByIdFiltered().FirstOrDefault();


        if (contact is null)
            return new(_localizer.Get("Contact_NotFound"), HttpStatusCode.NotFound);

        var NewDto = new ContactInfoGetDto(

            MetaTitle_Az: contact.MetaTitle_Az,
            MetaTitle_En: contact.MetaTitle_En,
            MetaTitle_Ru: contact.MetaTitle_Ru,

            MetaDescription_Az: contact.MetaDescription_Az,
            MetaDescription_En: contact.MetaDescription_En,
            MetaDescription_Ru: contact.MetaDescription_Ru,

            Keywords: contact.Keywords,

            Title_Az: contact.Title_Az,
            Title_En: contact.Title_En,
            Title_Ru: contact.Title_Ru,

            Phone: contact.Phone,
            Email: contact.Email,

            AddressAZ: contact.AddressAZ,
            AddressEN: contact.AddressEN,
            AddressRU: contact.AddressRU,

            MapIframeSrc: contact.MapIframeSrc
       );

        return new(_localizer.Get("Contact_Found"), NewDto, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> UpdateAsync(ContactInfoUpdateDto dto)
    {
        var contact = _contactRepository.GetByIdFiltered().FirstOrDefault();

        if (contact is null)
            return new(_localizer.Get("Contact_NotFound"), HttpStatusCode.NotFound);



        contact.Phone = Keep(contact.Phone, dto.Phone);
        contact.Email = Keep(contact.Email, dto.Email);
        contact.MapIframeSrc = Keep(contact.MapIframeSrc, dto.MapIframeSrc);

        contact.MetaTitle_Az = Keep(contact.MetaTitle_Az, dto.MetaTitle_Az);
        contact.MetaTitle_En = Keep(contact.MetaTitle_En, dto.MetaTitle_En);
        contact.MetaTitle_Ru = Keep(contact.MetaTitle_Ru, dto.MetaTitle_Ru);

        contact.MetaDescription_Az = Keep(contact.MetaDescription_Az, dto.MetaDescription_Az);
        contact.MetaDescription_En = Keep(contact.MetaDescription_En, dto.MetaDescription_En);
        contact.MetaDescription_Ru = Keep(contact.MetaDescription_Ru, dto.MetaDescription_Ru);

        contact.Keywords = Keep(contact.Keywords, dto.Keywords);

        contact.Title_Az = Keep(contact.Title_Az, dto.Title_Az);
        contact.Title_En = Keep(contact.Title_En, dto.Title_En);
        contact.Title_Ru = Keep(contact.Title_Ru, dto.Title_Ru);

        contact.AddressAZ = Keep(contact.AddressAZ, dto.AddressAZ);
        contact.AddressEN = Keep(contact.AddressEN, dto.AddressEN);
        contact.AddressRU = Keep(contact.AddressRU, dto.AddressRU);

        contact.UpdatedAt = DateTime.UtcNow; // varsa

        _contactRepository.Update(contact);
        await _contactRepository.SaveChangeAsync();

        return new(_localizer.Get("Contact_Updated"), true, HttpStatusCode.OK);
    }






    private static string Keep(string current, string? incoming)
    => string.IsNullOrWhiteSpace(incoming) ? current : incoming.Trim();

}
