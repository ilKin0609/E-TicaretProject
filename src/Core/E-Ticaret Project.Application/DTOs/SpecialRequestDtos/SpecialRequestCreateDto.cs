using Microsoft.AspNetCore.Http;

namespace E_Ticaret_Project.Application.DTOs.SpecialRequestDtos;

public record SpecialRequestCreateDto(

    string Name,
    string Surname,
    string Phone,
    string Email,
   IFormFile? File,
    string OrderAbout
);
