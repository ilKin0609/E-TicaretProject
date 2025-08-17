namespace E_Ticaret_Project.Application.DTOs.ProductDtos;

public record ListedReorderDto(Guid productId, List<ImageReorderDto> items);
