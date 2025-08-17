namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ProductUpdateAltDto(

    Guid imageId,
    string? altAz,
    string? altRu,
    string? altEn,
    bool autoFromMeta
);
