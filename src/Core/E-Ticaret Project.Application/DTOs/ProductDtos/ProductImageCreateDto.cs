using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductImageCreateDto(

        IFormFile File,  // YALNIZ create üçün vacib
        bool IsMain,
        int? SortOrder,             // göndərilməsə, server verəcək
        string? AltAz, 
        string? AltEn, 
        string? AltRu, 
        bool? AutoAltFromMeta 
    );
