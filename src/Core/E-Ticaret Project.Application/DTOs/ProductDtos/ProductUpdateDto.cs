using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductUpdateDto(

    Guid Id,
    Guid? CategoryId,
    string? SKU,
    decimal? PriceAZN,
    decimal? PartnerPriceAZN,
    string? TitleAz,
    string? TitleEn,
    string? TitleRu,
    string? DescAz,
    string? DescEn,
    string? DescRu,
    string? MetaTitleAz,
    string? MetaTitleEn,
    string? MetaTitleRu,
    string? MetaDescriptionAz,
    string? MetaDescriptionEn,
    string? MetaDescriptionRu,
    string? SlugAz,
     List<string>? Tags
);

