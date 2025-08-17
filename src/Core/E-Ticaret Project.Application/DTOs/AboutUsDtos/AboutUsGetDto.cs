namespace E_Ticaret_Project.Application.DTOs.AboutUsDtos;

public record AboutUsGetDto(

    string MetaTitle_Az,
    string MetaTitle_En,
    string MetaTitle_Ru,

    string MetaDescription_Az,
    string MetaDescription_En,
    string MetaDescription_Ru,

    string Keywords,

    string TitleAZ,
    string TitleEN,
    string TitleRU,

    string DescriptionAZ,
    string DescriptionEN,
    string DescriptionRU,

    string? ImageUrl
);
