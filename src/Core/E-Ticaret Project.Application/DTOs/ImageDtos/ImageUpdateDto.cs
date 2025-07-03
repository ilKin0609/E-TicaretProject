namespace E_Ticaret_Project.Application.DTOs.ImageDtos;

public record ImageUpdateDto(

    Guid Id,
    string Image_Url,
    bool? is_main

);
