namespace E_Ticaret_Project.Application.DTOs.ImageDtos;

public record ImageCreateDto(

    string Image_Url,
    bool is_main,
    Guid ProductId
);
