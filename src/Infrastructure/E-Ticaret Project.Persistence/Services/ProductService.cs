using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.FavoriteDtos;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using E_Ticaret_Project.Application.Shared;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_Ticaret_Project.Persistence.Services;

public class ProductService : IProductService
{
    private IProductRepository _productRepository { get; }
    private ICategoryRepository _categoryRepository { get; }
    private IHttpContextAccessor _httpContext { get; }
    private IFavoriteRepository _favoriteRepository { get; }

    public ProductService(IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IHttpContextAccessor httpContext,
        IFavoriteRepository favoriteRepository
        )
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _httpContext = httpContext;
        _favoriteRepository = favoriteRepository;
    }

    public async Task<BaseResponse<string>> CreateProduct(ProductCreateDto dto)
    {
        var categoryId = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (categoryId is null)
            return new("Category not found", HttpStatusCode.NotFound);

        var userId = CurrentUserHelper.GetUserId(_httpContext.HttpContext);
        if (userId is null)
            return new("User not found", HttpStatusCode.NotFound);
        var newProduct = new Product()
        {
            Tittle = dto.Tittle,
            Description = dto.Description,
            Price = dto.Price,
            Discount = dto.Discount,
            Rating = dto.Rating,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId
        };

        newProduct.OwnerId = userId;

        List<Image> productImages = new();

        if (dto.image is not null && dto.image.Any())
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var maxSizeInBytes = 5 * 1024 * 1024; // 5MB

            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            foreach (var formFile in dto.image)
            {
                var fileExtension = Path.GetExtension(formFile.FileName).ToLower();

                // Fayl tipi yoxla
                if (!allowedExtensions.Contains(fileExtension))
                    return new($"Invalid file type: {fileExtension}. Only .jpg, .jpeg, .png allowed.", HttpStatusCode.BadRequest);

                // Fayl ölçüsü yoxla
                if (formFile.Length > maxSizeInBytes)
                    return new($"File size exceeded. Max allowed size is 5 MB.", HttpStatusCode.BadRequest);

                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(imagesFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                productImages.Add(new Image
                {
                    Image_Url = $"/images/{fileName}"
                });
            }
        }

        newProduct.Images = productImages;

        await _productRepository.AddAsync(newProduct);
        await _productRepository.SaveChangeAsync();

        return new("Added successfully", true, HttpStatusCode.Created);

    }

    public async Task<BaseResponse<ProductUpdateDto>> UpdateProduct(ProductUpdateDto dto)
    {
        var product = await _productRepository.GetByIdAsync(dto.Id);
        if (product is null)
            return new("Product not found", HttpStatusCode.NotFound);

        product.Tittle = dto.Title;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Discount = dto.Discount;
        product.Rating = dto.Rating;

        if (dto.Stock.HasValue)
            product.Stock = dto.Stock.Value;

        if (dto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value);
            if (category is null)
                return new("Category is not found", HttpStatusCode.NotFound);

            product.CategoryId = category.Id;
        }

        if (dto.Images is not null && dto.Images.Any())
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var maxSizeInBytes = 5 * 1024 * 1024; // 5MB

            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            foreach (var formFile in dto.Images)
            {
                if (string.IsNullOrWhiteSpace(formFile.FileName))
                    return new("Invalid file name.", HttpStatusCode.BadRequest);

                var extension = Path.GetExtension(formFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return new($"Invalid file type: {extension}. Only .jpg, .jpeg, .png allowed.", HttpStatusCode.BadRequest);

                if (formFile.Length > maxSizeInBytes)
                    return new("File size exceeded. Max allowed size is 5 MB.", HttpStatusCode.BadRequest);

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(imagesFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                product.Images.Add(new Image
                {
                    Image_Url = $"/images/{fileName}"
                });
            }
        }

        _productRepository.Update(product);
        await _productRepository.SaveChangeAsync();

        return new("Product updated successfully", true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> DeleteProduct(Guid productId)
    {

        var product = await _productRepository
        .GetByIdFiltered(
            predicate: p => p.Id == productId,
            include: [p => p.Images]
        )
        .FirstOrDefaultAsync();

        if (product is null)
            return new("Product not found", HttpStatusCode.NotFound);

        if (product.Images is not null && product.Images.Any())
        {
            foreach (var image in product.Images)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Image_Url.TrimStart('/'));

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }


        _productRepository.Delete(product);
        await _productRepository.SaveChangeAsync();
        return new("Product deleted successfully", true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> AddProductImage(Guid productId, List<IFormFile> images)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            return new("Product is not found", HttpStatusCode.NotFound);

        if (images is null)
            return new("Please insert photo", HttpStatusCode.BadRequest);

        List<Image> productImages = new();

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var maxSizeInBytes = 5 * 1024 * 1024; // 5MB

        var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        if (!Directory.Exists(imagesFolder))
            Directory.CreateDirectory(imagesFolder);

        foreach (var formFile in images)
        {
            var fileExtension = Path.GetExtension(formFile.FileName).ToLower();

            // Fayl tipi yoxla
            if (!allowedExtensions.Contains(fileExtension))
                return new($"Invalid file type: {fileExtension}. Only .jpg, .jpeg, .png allowed.", HttpStatusCode.BadRequest);

            // Fayl ölçüsü yoxla
            if (formFile.Length > maxSizeInBytes)
                return new($"File size exceeded. Max allowed size is 5 MB.", HttpStatusCode.BadRequest);

            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(imagesFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            product.Images.Add(new Image
            {
                Image_Url = $"/images/{fileName}"
            });
        }
        _productRepository.Update(product);
        await _productRepository.SaveChangeAsync();

        return new("Images successfully added",true,HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> RemoveProductImage(Guid imageId)
    {
        var image=await _productRepository.GetImageByIdAsync(imageId);

        if(image is null)
            return new("Image is not found",HttpStatusCode.NotFound);

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Image_Url.TrimStart('/'));

        if (File.Exists(fullPath))
            File.Delete(fullPath);

         _productRepository.DeleteAsync(image);
        await _productRepository.SaveChangeAsync();
        return new("Image successfully deleted", true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> AddProductFavorite(FavoriteCreateDto dto)
    {
        var alreadyFavorite = await _favoriteRepository.AnyAsync(f =>
            f.ProductId == dto.ProductId && f.UserId == dto.UserId);

        if (alreadyFavorite)
            return new("Already favorite", HttpStatusCode.BadRequest);

        var favorite = new Favorite
        {
            ProductId = dto.ProductId,
            UserId = dto.UserId
        };

        await _favoriteRepository.AddAsync(favorite);
        await _favoriteRepository.SaveChangeAsync();

        return new("Product added to favorites", HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string>> RemoveProductFavorite(Guid Id)
    {
        var favorite = await _favoriteRepository.GetByIdAsync(Id);
        if (favorite is null)
            return new("Favorite not found", HttpStatusCode.NotFound);

        _favoriteRepository.Delete(favorite);
        await _favoriteRepository.SaveChangeAsync();

        return new("Favorite deleted successfully", HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductGetDto>>> GetAllProduct()
    {
        var products = await _productRepository
        .GetAllFiltered(include: [p => p.Images])
        .ToListAsync();

        if (!products.Any())
            return new("No products found", HttpStatusCode.NotFound);

        var productDtos = products.Select(p => new ProductGetDto(
            Id: p.Id,
            Title: p.Tittle,
            Description: p.Description,
            Price: p.Price,
            Discount: p.Discount,
            Rating: p.Rating ?? 0,
            Stock: p.Stock,
            CategoryId: p.CategoryId,
            OwnerId: p.OwnerId,
            ImageUrls: p.Images?.Select(img => img.Image_Url).ToList()
        )).ToList();

        return new("All products", productDtos, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<ProductGetDto>> GetByIdProduct(Guid productId)
    {
        var product = await _productRepository.GetByIdFiltered(
            predicate: p => p.Id == productId,
            include: [p => p.Images]
            ).FirstOrDefaultAsync();

        if (product is null)
            return new("No product found", HttpStatusCode.NotFound);

        var productDto = new ProductGetDto(
            Id: product.Id,
            Title: product.Tittle,
            Description: product.Description,
            Price: product.Price,
            Discount: product.Discount,
            Rating: product.Rating ?? 0,
            Stock: product.Stock,
            CategoryId: product.CategoryId,
            OwnerId: product.OwnerId,
            ImageUrls: product.Images?.Select(img => img.Image_Url).ToList()
            );

        return new("product found", productDto, HttpStatusCode.OK);


    }

    public async Task<BaseResponse<List<ProductGetDto>>> GetByTitleProduct(string Title)
    {
        var products = await _productRepository
        .GetAllFiltered(
            predicate: p => p.Tittle == Title,
            include: [p => p.Images]
        )
        .ToListAsync();

        if (!products.Any())
            return new($"{Title} product is not found", HttpStatusCode.NotFound);


        var dtoProduct = new List<ProductGetDto>();
        foreach (var product in products)
        {
            dtoProduct.Add(new ProductGetDto(
                Id: product.Id,
                Title: product.Tittle,
                Description: product.Description,
                Price: product.Price,
                Discount: product.Discount,
                Rating: product.Rating ?? 0,
                Stock: product.Stock,
                CategoryId: product.CategoryId,
                OwnerId: product.OwnerId,
                ImageUrls: product.Images?.Select(img => img.Image_Url).ToList()
             ));
        }

        return new($"All products which title is:{Title} ", dtoProduct, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductGetDto>>> GetMyProducts(string userId)
    {


        var products = await _productRepository.GetAllFiltered(
            predicate: p => p.OwnerId == userId,
            include: [
                        p => p.Images,
                        p => p.Category,
                        p => p.Comments
                     ]
            ).ToListAsync();
        if (!products.Any())
            return new("You have not added any products yet.", HttpStatusCode.NotFound);

        var MyProducts = new List<ProductGetDto>();

        foreach (var product in products)
        {
            MyProducts.Add(new ProductGetDto(
                    Id: product.Id,
                    Title: product.Tittle,
                    Description: product.Description,
                    Price: product.Price,
                    Discount: product.Discount,
                    Rating: product.Rating ?? 0,
                    Stock: product.Stock,
                    CategoryId: product.CategoryId,
                    OwnerId: product.OwnerId,
                    ImageUrls: product.Images?.Select(img => img.Image_Url).ToList()
                ));
        }
        return new("All your products", MyProducts, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductGetDto>>> GetDiscountProducts()
    {
        var discountProducts = await _productRepository.GetAllFiltered(
            predicate: p => p.Discount > 0,
             include: [p => p.Images]
            ).ToListAsync();

        if (!discountProducts.Any())
            return new("There are no discounted products.", HttpStatusCode.NotFound);

        var products = new List<ProductGetDto>();
        foreach (var product in discountProducts)
        {
            products.Add(new ProductGetDto(
                    Id: product.Id,
                    Title: product.Tittle,
                    Description: product.Description,
                    Price: product.Price,
                    Discount: product.Discount,
                    Rating: product.Rating ?? 0,
                    Stock: product.Stock,
                    CategoryId: product.CategoryId,
                    OwnerId: product.OwnerId,
                    ImageUrls: product.Images?.Select(img => img.Image_Url).ToList())
                );
        }
        return new("All discount products", products, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductGetDto>>> GetByCategoryProducts(Guid categoryId)
    {
        var allCategories = await _categoryRepository.GetAll().ToListAsync();
        if (!allCategories.Any())
            return new("Categories not found", HttpStatusCode.NotFound);

        var categoryIds = GetAllSubCategoryIds(allCategories, categoryId);

        var products = await _productRepository
        .GetAllFiltered(
            predicate: p => categoryIds.Contains(p.CategoryId),
            include: [
                p => p.Category,
                p => p.Images
            ])
        .ToListAsync();

        if (!products.Any())
            return new("No products found for this category.", HttpStatusCode.NotFound);

        var productDtos = new List<ProductGetDto>();

        foreach (var product in products)
        {
            productDtos.Add(new ProductGetDto(
                    Id: product.Id,
                    Title: product.Tittle,
                    Description: product.Description,
                    Price: product.Price,
                    Discount: product.Discount,
                    Rating: product.Rating ?? 0,
                    Stock: product.Stock,
                    CategoryId: product.CategoryId,
                    OwnerId: product.OwnerId,
                    ImageUrls: product.Images?.Select(img => img.Image_Url).ToList()
                ));
        }
        return new("All products", productDtos, HttpStatusCode.OK);
    }




    private List<Guid> GetAllSubCategoryIds(List<Domain.Entities.Category> allCategories, Guid rootCategoryId)
    {
        var result = new List<Guid> { rootCategoryId };

        void FindChildren(Guid parentId)
        {
            var children = allCategories.Where(c => c.ParentCategoryId == parentId).ToList();

            foreach (var child in children)
            {
                result.Add(child.Id);
                FindChildren(child.Id); // recursive tapırıq
            }
        }

        FindChildren(rootCategoryId);
        return result;
    }

}
