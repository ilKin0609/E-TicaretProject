namespace E_Ticaret_Project.Application.DTOs.ContactInfoDtos;

public record ContactInfoGetDto(

    string MetaTitle_Az,
    string MetaTitle_En,
    string MetaTitle_Ru,

    string MetaDescription_Az,
    string MetaDescription_En,
    string MetaDescription_Ru,

    string Keywords,

    string Title_Az,
    string Title_En,
    string Title_Ru,

    string AddressAZ,
    string AddressEN,
    string AddressRU,

    string Phone,
    string Email,
    string MapIframeSrc

);
