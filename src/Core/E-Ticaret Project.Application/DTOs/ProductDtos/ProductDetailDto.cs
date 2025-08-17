namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductDetailDto(

    Guid Id,
    string SID,
    string SKU,
    Guid? CategoryId,
    string? CategoryName,    
    string TitleAz,
    string? TitleEn,
    string? TitleRu,
    string? DescAz,
    string? DescEn,
    string? DescRu,
    decimal? PriceAZN,
    decimal? PartnerPriceAZN,
    string? SlugAz,
    string? Tags,
    List<ProductImageDto> Images
);
