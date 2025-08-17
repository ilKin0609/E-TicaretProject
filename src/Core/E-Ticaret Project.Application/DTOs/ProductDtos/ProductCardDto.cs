namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductCardDto(

     Guid Id,
    string SID,
    string? SlugAz,
    string TitleAZ,          
    string TitleRU,          
    string TitleEN,          
    string? ShortDescAZ,     
    string? ShortDescRU,     
    string? ShortDescEN,     
    decimal? PriceUser,
    decimal? PricePartnor,
    string? MainImageUrl   
);
