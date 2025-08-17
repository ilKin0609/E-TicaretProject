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

        if (contact is null)
            return new(_localizer.Get("Contact_NotFound"), HttpStatusCode.NotFound);

        // partial update – null və ya boş olanlara toxunulmur
        if (!string.IsNullOrWhiteSpace(dto.Phone))
            contact.Phone = dto.Phone;

        if (!string.IsNullOrWhiteSpace(dto.Email))
            contact.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.MapIframeSrc))
            contact.MapIframeSrc = dto.MapIframeSrc;

        if (!string.IsNullOrWhiteSpace(dto.MetaTitle_Az))
            contact.MetaTitle_Az = dto.MetaTitle_Az;

        if (!string.IsNullOrWhiteSpace(dto.MetaTitle_En))
            contact.MetaTitle_En = dto.MetaTitle_En;

        if (!string.IsNullOrWhiteSpace(dto.MetaTitle_Ru))
            contact.MetaTitle_Ru = dto.MetaTitle_Ru;

        if (!string.IsNullOrWhiteSpace(dto.MetaDescription_Az))
            contact.MetaDescription_Az = dto.MetaDescription_Az;

        if (!string.IsNullOrWhiteSpace(dto.MetaDescription_En))
            contact.MetaDescription_En = dto.MetaDescription_En;

        if (!string.IsNullOrWhiteSpace(dto.MetaDescription_Ru))
            contact.MetaDescription_Ru = dto.MetaDescription_Ru;

        if (!string.IsNullOrWhiteSpace(dto.Keywords))
            contact.Keywords = dto.Keywords;

        if (!string.IsNullOrWhiteSpace(dto.Title_Az))
            contact.Title_Az = dto.Title_Az;

        if (!string.IsNullOrWhiteSpace(dto.Title_En))
            contact.Title_En = dto.Title_En;

        if (!string.IsNullOrWhiteSpace(dto.Title_Ru))
            contact.Title_Ru = dto.Title_Ru;

        if (!string.IsNullOrWhiteSpace(dto.AddressAZ))
            contact.AddressAZ = dto.AddressAZ;

        if (!string.IsNullOrWhiteSpace(dto.AddressEN))
            contact.AddressEN = dto.AddressEN;

        if (!string.IsNullOrWhiteSpace(dto.AddressRU))
            contact.AddressRU = dto.AddressRU;

        _contactRepository.Update(contact);
        await _contactRepository.SaveChangeAsync();

        return new(_localizer.Get("Contact_Updated"), true, HttpStatusCode.OK);
    }

}
