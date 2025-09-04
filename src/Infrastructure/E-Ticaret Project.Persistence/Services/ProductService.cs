using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace E_Ticaret_Project.Persistence.Services;

public class ProductService : IProductService
{
    private IProductRepository _productRepository { get; }
    private ICategoryService _categoryService { get; }
    private ITagRepository _tagRepository { get; }
    private ILocalizationService _localizer { get; }
    private ICloudinaryService _cloud { get; }
    private IProductImageRepository _productImageRepository { get; }
    private IHttpContextAccessor _ctx { get; }


    public ProductService(IProductRepository productRepository,
        ICategoryService categoryService,
        ITagRepository tagRepository,
        ILocalizationService localizer,
        ICloudinaryService cloud,
        IProductImageRepository productImageRepository,
        IHttpContextAccessor ctx)
    {
        _productRepository = productRepository;
        _categoryService = categoryService;
        _localizer = localizer;
        _tagRepository = tagRepository;
        _cloud = cloud;
        _productImageRepository = productImageRepository;
        _ctx = ctx;
    }

    public async Task<BaseResponse<string>> CreateAsync(ProductCreateDto dto)
    {


        var skuExists = await _productRepository.AnyAsync(p => p.SKU == dto.SKU);
        if (skuExists)
            return new(_localizer.Get("Product_SKU_AlreadyExists"), HttpStatusCode.Conflict);


        var sid = await GenerateUniqueSidAsync(8);

        string? slugAz = null;
        if (!string.IsNullOrWhiteSpace(dto.SlugAz))
            slugAz = NormalizeSlug(dto.SlugAz!);
        else
            slugAz = NormalizeSlug(dto.TitleAz);

        if (string.IsNullOrWhiteSpace(slugAz)) slugAz = null;
        else slugAz = await MakeUniqueProductSlugAsync(slugAz);



        var p = new Product
        {
            SID = sid,
            SKU = dto.SKU,
            PriceAZN = dto.PriceAZN,
            PartnerPriceAZN = dto.PartnerPriceAZN,
            TitleAz = dto.TitleAz,
            TitleEn = dto.TitleEn,
            TitleRu = dto.TitleRu,
            DescAz = dto.DescAz,
            DescEn = dto.DescEn,
            DescRu = dto.DescRu,
            MetaTitleAz = dto.MetaTitleAz,
            MetaTitleEn = dto.MetaTitleEn,
            MetaTitleRu = dto.MetaTitleRu,
            MetaDescriptionAz = dto.MetaDescriptionAz,
            MetaDescriptionEn = dto.MetaDescriptionEn,
            MetaDescriptionRu = dto.MetaDescriptionRu,
            SlugAz = slugAz,
            ProductTags = new List<ProductTag>(),
            Images = new List<ProductImage>()
        };
        if (dto.CategoryId is not null)
        {
            var leaf = await _categoryService.IsLeafAsync(dto.CategoryId.Value);
            if (!leaf.Success)
                return new("Category_MustBeLeaf", HttpStatusCode.BadRequest);

            p.CategoryId = dto.CategoryId;

        }

        // 1-ci save: Product (Id lazımdırsa)

        // Tag-lar (gəlmişsə)
        var tags = NormalizeTags(dto.Tags);
        if (tags.Count > 0)
        {
            await AttachTagsAsync(p, tags);

        }
       

        if (dto.Images is not null && dto.Images.Count > 0)
        {
            // 1) Yalnız faylı olan itemləri götür, SortOrder varsa ona görə sırala
            var images = dto.Images
                .Where(i => i.File is { Length: > 0 })
                .OrderBy(i => i.SortOrder ?? int.MaxValue) // SortOrder verilməyənlər sonda
                .ToList();
            var nextOrder = 10;                  // SortOrder gəlməyibsə avtomatik artım

            foreach (var img in images)
            {
                // 2) Fayl və tip yoxlaması
                if (!IsValidImage(img.File))
                    return new(_localizer.Get("Image_File_TypeNotAllowed"), HttpStatusCode.BadRequest);

                // 3) Cloudinary-ə yüklə
                var (url, publicId) = await _cloud.UploadImageAsync(img.File, $"ecommerce/products/{sid}");
                if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(publicId))
                    return new(_localizer.Get("Image_Upload_Failed"), HttpStatusCode.BadRequest);

                
                bool isMain = img.IsMain;

               

                // 6) DB obyektini əlavə et
                p.Images.Add(new ProductImage
                {
                    
                    Url = url,
                    PublicId = publicId,
                    IsMain = isMain,
                    SortOrder = img.SortOrder ?? (nextOrder += 10),
                    AltAz = img.AltAz,
                    AltRu = img.AltRu,
                    AltEn = img.AltEn
                });

            }

        }
        await _productRepository.AddAsync(p);            
        await _productRepository.SaveChangeAsync();

        return new(_localizer.Get("Product_Created"), true, HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string>> UpdateAsync(ProductUpdateDto dto)
    {
        var p = await _productRepository.GetByIdFiltered(
                x => x.Id == dto.Id,
                include: new Expression<Func<Product, object>>[] { x => x.ProductTags },
                isTracking: true
            ).FirstOrDefaultAsync();

        if (p is null)
            return new(_localizer.Get("Product_NotFound"), HttpStatusCode.NotFound);

        // Kateqoriya (leaf)
        if (dto.CategoryId.HasValue && dto.CategoryId != p.CategoryId)
        {
            if (dto.CategoryId != null)
            {
                var leaf = await _categoryService.IsLeafAsync(dto.CategoryId.Value);
                if (!leaf.Success)
                    return new("Category has children please select child", HttpStatusCode.BadRequest);
            }
            p.CategoryId = dto.CategoryId; // null ola bilər
        }

        // SKU
        if (!string.IsNullOrWhiteSpace(dto.SKU) && !string.Equals(dto.SKU, p.SKU, StringComparison.Ordinal))
        {
            var exists = await _productRepository.AnyAsync(x => x.SKU == dto.SKU && x.Id != p.Id);
            if (exists) return new(_localizer.Get("Product_SKU_AlreadyExists"), HttpStatusCode.Conflict);
            p.SKU = dto.SKU!;
        }

        // Price
        if (dto.PriceAZN.HasValue)
        {
            if (dto.PriceAZN.Value < 0) return new(_localizer.Get("Product_Invalid_Price"), HttpStatusCode.BadRequest);
            p.PriceAZN = dto.PriceAZN.Value;
        }
        if (dto.PartnerPriceAZN.HasValue)
        {
            if (dto.PartnerPriceAZN.Value < 0) return new(_localizer.Get("Product_Invalid_Price"), HttpStatusCode.BadRequest);
            p.PartnerPriceAZN = dto.PartnerPriceAZN.Value;
        }

        // Titles / Descs
        if (dto.TitleAz is not null)
        {
            if (string.IsNullOrWhiteSpace(dto.TitleAz))
                return new(_localizer.Get("Product_TitleAz_Required"), HttpStatusCode.BadRequest);
            p.TitleAz = dto.TitleAz;
        }
        if (dto.TitleEn is not null) p.TitleEn = dto.TitleEn;
        if (dto.TitleRu is not null) p.TitleRu = dto.TitleRu;

        if (dto.DescAz is not null) p.DescAz = dto.DescAz;
        if (dto.DescEn is not null) p.DescEn = dto.DescEn;
        if (dto.DescRu is not null) p.DescRu = dto.DescRu;

        // Meta
        if (dto.MetaTitleAz is not null) p.MetaTitleAz = dto.MetaTitleAz;
        if (dto.MetaTitleEn is not null) p.MetaTitleEn = dto.MetaTitleEn;
        if (dto.MetaTitleRu is not null) p.MetaTitleRu = dto.MetaTitleRu;
        if (dto.MetaDescriptionAz is not null) p.MetaDescriptionAz = dto.MetaDescriptionAz;
        if (dto.MetaDescriptionEn is not null) p.MetaDescriptionEn = dto.MetaDescriptionEn;
        if (dto.MetaDescriptionRu is not null) p.MetaDescriptionRu = dto.MetaDescriptionRu;

        // Slug (yalnız göndərilərsə)
        if (dto.SlugAz is not null)
        {
            if (string.IsNullOrWhiteSpace(dto.SlugAz))
                p.SlugAz = null;
            else
                p.SlugAz = await MakeUniqueProductSlugAsync(NormalizeSlug(dto.SlugAz), p.Id);
        }

        // TAGS:
        if (dto.Tags is not null)
        {
            var desired = NormalizeTags(dto.Tags); // null-dan fərqli: ya boş (hamısını sil), ya da siyahı
            await ReplaceTagsAsync(p, desired);
        }

        _productRepository.Update(p);
        await _productRepository.SaveChangeAsync();

        return new(_localizer.Get("Product_Updated"), true, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> DeleteAsync(Guid id)
    {
        // 1) Məhsulu şəkillər və tag-join-larla birlikdə yüklə (tracking ON)
        var p = await _productRepository.GetByIdFiltered(
                    x => x.Id == id,
                    include:
                    [
                    x => x.Images,
                    x => x.ProductTags
                    ],
                    isTracking: true
                ).FirstOrDefaultAsync();

        if (p is null)
            return new(_localizer.Get("Product_NotFound"), HttpStatusCode.NotFound);

        // 2) Cloudinary-dən bütün şəkilləri sil (best-effort)
        foreach (var img in p.Images.ToList())
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(img.PublicId))
                    await _cloud.DeleteImageAsync(img.PublicId);
            }
            catch
            {
                // istəsəniz loglayın; DB silinməsinə mane olmayaq
            }
        }

        // 3) DB-dən məhsulu fiziki sil (cascade -> Images, ProductTags da silinəcək)
        _productRepository.HardDelete(p);
        await _productRepository.SaveChangeAsync();

        // 4) (Opsional) istifadə olunmayan tag-ları təmizlə
        var orphanTags = await _tagRepository.GetAll()
            .Where(t => !t.ProductTags.Any())
            .ToListAsync();

        if (orphanTags.Count > 0)
        {
            foreach (var t in orphanTags)
                _tagRepository.HardDelete(t);

            await _tagRepository.SaveChangeAsync();
        }

        return new(_localizer.Get("Product_Deleted"), true, HttpStatusCode.OK);
    }



    public async Task<BaseResponse<ProductDetailDto>> GetDetailByIdAsync(Guid id)
    {
        var p = await _productRepository.GetAllFiltered(
        x => x.Id == id,
        include: new Expression<Func<Product, object>>[]
        {
            x => x.Images, x => x.Category, x => x.ProductTags
        },
        isTracking: false
    ).FirstOrDefaultAsync();

        if (p is null)
            return new(_localizer.Get("Product_NotFound"), HttpStatusCode.NotFound);

        // Tag adlarını 1 sorğuda götürürük
        var tags = await _tagRepository.GetAll()
            .Where(t => t.ProductTags.Any(pt => pt.ProductId == p.Id))
            .OrderBy(t => t.Name)
            .Select(t => t.Name)
            .ToListAsync();

        var images = (p.Images ?? new List<ProductImage>())
            .OrderByDescending(i => i.IsMain)
            .ThenBy(i => i.SortOrder)
            .Select(i => new ProductImageDto(
                i.Id, i.Url, i.IsMain, i.SortOrder,
                i.AltAz, i.AltRu, i.AltEn
            ))
            .ToList();

        // Tək CategoryName (lokalizə)
        var categoryName = p.Category == null
            ? null
            : GetLocalizedName(p.Category.NameAz, p.Category.NameRu, p.Category.NameEn);

        // Tags-i vergüllə birləşdir
        var tagsCsv = tags.Count == 0 ? null : string.Join(", ", tags);

        var dto = new ProductDetailDto(
            p.Id,
            p.SID,
            p.SKU,
            p.CategoryId,
            categoryName,
            p.TitleAz, p.TitleEn, p.TitleRu,
            p.DescAz, p.DescEn, p.DescRu,
            p.PriceAZN, p.PartnerPriceAZN,
            p.SlugAz,
            tagsCsv,
            images
        );

        return new(_localizer.Get("Product_Found"), dto, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<ProductDetailDto>> GetDetailBySidAsync(string sid)
    {
        sid = sid?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(sid))
            return new(_localizer.Get("Product_NotFound"), HttpStatusCode.NotFound);

        var p = await _productRepository.GetAllFiltered(
            x => x.SID == sid,
            include: new Expression<Func<Product, object>>[]
            {
            x => x.Images, x => x.Category, x => x.ProductTags
            },
            isTracking: false
        ).FirstOrDefaultAsync();

        if (p is null)
            return new(_localizer.Get("Product_NotFound"), HttpStatusCode.NotFound);

        var tags = await _tagRepository.GetAll()
            .Where(t => t.ProductTags.Any(pt => pt.ProductId == p.Id))
            .OrderBy(t => t.Name)
            .Select(t => t.Name)
            .ToListAsync();

        var images = (p.Images ?? new List<ProductImage>())
            .OrderByDescending(i => i.IsMain)
            .ThenBy(i => i.SortOrder)
            .Select(i => new ProductImageDto(
                i.Id, i.Url, i.IsMain, i.SortOrder,
                i.AltAz, i.AltRu, i.AltEn
            ))
            .ToList();

        var categoryName = p.Category == null
            ? null
            : GetLocalizedName(p.Category.NameAz, p.Category.NameRu, p.Category.NameEn);

        var tagsCsv = tags.Count == 0 ? null : string.Join(", ", tags);

        var dto = new ProductDetailDto(
            p.Id,
            p.SID,
            p.SKU,
            p.CategoryId,
            categoryName,
            p.TitleAz, p.TitleEn, p.TitleRu,
            p.DescAz, p.DescEn, p.DescRu,
            p.PriceAZN, p.PartnerPriceAZN,
            p.SlugAz,
            tagsCsv,
            images
        );

        return new(_localizer.Get("Product_Found"), dto, HttpStatusCode.OK);
    }


    public async Task<BaseResponse<List<ProductCardDto>>> GetByCategoryAsync(Guid categoryId)
    {
        var list = await _productRepository.GetAll()
        .Where(p => p.CategoryId == categoryId)
        .Include(p => p.Images)
        .OrderByDescending(p => p.CreatedAt)
        .ToListAsync();

        var dtos = list.Select(p => new ProductCardDto(
            p.Id,
            p.SID,
            p.SlugAz,
            p.TitleAz,
            p.TitleRu ?? p.TitleAz,
            p.TitleEn ?? p.TitleAz,
            p.DescAz,
            p.DescRu,
            p.DescEn,
            p.PriceAZN,
            p.PartnerPriceAZN,
            p.Images
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.SortOrder)
                .Select(i => i.Url)
                .FirstOrDefault()
        )).ToList();

        return dtos.Count == 0
        ? new(_localizer.Get("Products_NotFound"), HttpStatusCode.NotFound)
        : new(_localizer.Get("Products_Found"), dtos, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductCardDto>>> GetByTagId(Guid tagId)
    {
        var tag = await _tagRepository.GetByIdAsync(tagId);

        if (tag is null)
            return new(_localizer.Get("Tag_NotFound"), HttpStatusCode.NotFound);

        var products = await _productRepository.GetAllFiltered(
            predicate: p => p.ProductTags.Any(pt => pt.TagId == tagId),
            include: new Expression<Func<Product, object>>[] { p => p.Images },
            OrderBy: p => p.CreatedAt,
            isOrderBy: false // DESC
        )
        .ToListAsync();

        if (products.Count == 0)
            return new(_localizer.Get("Products_NotFound"), HttpStatusCode.NotFound);

        var cards = products.Select(p =>
        {
            var main = p.Images?
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.SortOrder)
                .FirstOrDefault();

            return new ProductCardDto(
                p.Id,
                p.SID,
                p.SlugAz,
                p.TitleAz,
                p.TitleRu ?? p.TitleAz,
                p.TitleEn ?? p.TitleAz,
                p.DescAz,
                p.DescRu,
                p.DescEn,
                p.PriceAZN,
                p.PartnerPriceAZN,
                main?.Url
            );
        }).ToList();

        return new(_localizer.Get("Products_Found"), cards, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<ProductCardDto>>> GetByTagAsync(string tag)
    {
        var slug = NormalizeSlug(tag);
        if (string.IsNullOrWhiteSpace(slug))
            return new(_localizer.Get("Products_NotFound"), HttpStatusCode.NotFound);

        var list = await _productRepository.GetAllFiltered(
                predicate: p => p.ProductTags.Any(pt => pt.Tag.Slug == slug),
                include: new Expression<Func<Product, object>>[] { p => p.Images },
                OrderBy: p => p.CreatedAt,
                isOrderBy: false // DESC
            )
            .ToListAsync();

        if (list.Count == 0)
            return new(_localizer.Get("Products_NotFound"), HttpStatusCode.NotFound);

        // Nəticə varsa → həmin TAG üçün populyarlıq +1
        await _categoryService.IncrementKeywordSearchAsync(tag);

        var cards = list.Select(p =>
        {
            var main = p.Images?.Where(i => !i.IsDeleted)
                                .OrderByDescending(i => i.IsMain)
                                .ThenBy(i => i.SortOrder)
                                .FirstOrDefault();

            return new ProductCardDto(
                p.Id,
                p.SID,
                p.SlugAz,
                p.TitleAz,
                p.TitleRu ?? p.TitleAz,
                p.TitleEn ?? p.TitleAz,
                p.DescAz,
                p.DescRu,
                p.DescEn,
                p.PriceAZN,
                p.PartnerPriceAZN,
                main?.Url
            );
        }).ToList();

        return new(_localizer.Get("Products_Found"), cards, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductCardDto>>> GetByTagsAsync(IEnumerable<string> tags)
    {
        var pairs = (tags ?? Enumerable.Empty<string>())
       .Select(t => new { Raw = (t ?? "").Trim(), Slug = NormalizeSlug(t) })
       .Where(x => !string.IsNullOrWhiteSpace(x.Slug))
       .GroupBy(x => x.Slug, StringComparer.OrdinalIgnoreCase)
       .ToDictionary(g => g.Key, g => g.First().Raw, StringComparer.OrdinalIgnoreCase);

        if (pairs.Count == 0)
            return new(_localizer.Get("Products_NotFound"), HttpStatusCode.NotFound);

        var slugs = pairs.Keys.ToList();

        // ANY (OR) məntiqi + şəkilləri yüklə + dublikatları id-ə görə təmizlə
        var list = await _productRepository.GetAllFiltered(
                predicate: p => p.ProductTags.Any(pt => slugs.Contains(pt.Tag.Slug)),
                include: new Expression<Func<Product, object>>[] { p => p.Images },
                OrderBy: p => p.CreatedAt,
                isOrderBy: false // DESC
            )
            .AsNoTracking()
            .GroupBy(p => p.Id)
            .Select(g => g.First())
            .ToListAsync();

        if (list.Count == 0)
            return new(_localizer.Get("Products_NotFound"), HttpStatusCode.NotFound);

        // Hansı sluq-lar həqiqətən nəticə verib? → tək sorğu ilə tapırıq
        var matchedSlugs = await _productRepository.GetAll()
            .Where(p => p.ProductTags.Any(pt => slugs.Contains(pt.Tag.Slug)))
            .SelectMany(p => p.ProductTags
                .Where(pt => slugs.Contains(pt.Tag.Slug))
                .Select(pt => pt.Tag.Slug))
            .Distinct()
            .ToListAsync();

        if (matchedSlugs.Count > 0)
        {
            var matchedRaw = matchedSlugs
                .Select(s => pairs.TryGetValue(s, out var raw) ? raw : null)
                .Where(raw => !string.IsNullOrWhiteSpace(raw))
                .ToList();

            if (matchedRaw.Count > 0)
                await _categoryService.IncrementKeywordSearchManyAsync(matchedRaw);
        }

        // Manual map → ProductCardDto
        var cards = list.Select(p =>
        {
            var main = p.Images?
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.SortOrder)
                .FirstOrDefault();

            return new ProductCardDto(
                p.Id,
                p.SID,
                p.SlugAz,
                p.TitleAz,
                p.TitleRu ?? p.TitleAz,
                p.TitleEn ?? p.TitleAz,
                p.DescAz,
                p.DescRu,
                p.DescEn,
                p.PriceAZN,
                p.PartnerPriceAZN,
                main?.Url
            );
        }).ToList();

        return new(_localizer.Get("Products_Found"), cards, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductCardDto>>> SearchAsync(string q)
    {
        q = (q ?? "").Trim();
        if (q.Length == 0)
            return new(_localizer.Get("Products_NotFound"), HttpStatusCode.NotFound);

        var list = await _productRepository.GetAll()
            .Where(p =>
                (p.TitleAz != null && p.TitleAz.Contains(q)) ||
                (p.TitleRu != null && p.TitleRu.Contains(q)) ||
                (p.TitleEn != null && p.TitleEn.Contains(q)) ||
                (p.DescAz != null && p.DescAz.Contains(q)) ||
                (p.DescRu != null && p.DescRu.Contains(q)) ||
                (p.DescEn != null && p.DescEn.Contains(q))
            )
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var dtos = list.Select(p => new ProductCardDto(
            p.Id,
            p.SID,
            p.SlugAz,
            p.TitleAz,
            p.TitleRu ?? p.TitleAz,
            p.TitleEn ?? p.TitleAz,
            p.DescAz,
            p.DescRu,
            p.DescEn,
            p.PriceAZN,
            p.PartnerPriceAZN,
            p.Images
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.SortOrder)
                .Select(i => i.Url)
                .FirstOrDefault()
        )).ToList();

        return dtos.Count == 0
            ? new(_localizer.Get("Products_NotFound"), HttpStatusCode.NotFound)
            : new(_localizer.Get("Products_Found"), dtos, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductCardDto>>> SmartSearchAsync(string q)
    {
        q = (q ?? "").Trim();
        if (string.IsNullOrWhiteSpace(q))
            return new BaseResponse<List<ProductCardDto>>("Products_NotFound", HttpStatusCode.OK)
            {
                Data = new List<ProductCardDto>() 
            };

        var seen = new HashSet<Guid>(); 
        var ranked = new List<(ProductCardDto Item, int Rank)>();

        
        var skuRes = await GetBySKUAsync(q); 
        if (skuRes?.Data != null)
        {
            var p = skuRes.Data;
            if (AddUnique(seen, p))
                ranked.Add((p, 3));
        }

       
        var tags = ParseTags(q);
        if (tags.Length > 0)
        {
            var tagRes = await GetByTagsAsync(tags); 
            if (tagRes?.Data != null)
                foreach (var p in tagRes.Data)
                    if (AddUnique(seen, p))
                        ranked.Add((p, 2));
        }

        
        var textRes = await SearchAsync(q); 
        if (textRes?.Data != null)
        {
            foreach (var p in textRes.Data)
                if (AddUnique(seen, p))
                    ranked.Add((p, 1));
        }

        var ordered = ranked
            .OrderByDescending(x => x.Rank)
            .Select(x => x.Item)
            .ToList();

        return new BaseResponse<List<ProductCardDto>>("Products_Found", ordered, HttpStatusCode.OK);

        static string[] ParseTags(string input)
        {
            var split = input
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return split.Length > 0 ? split : new[] { input.Trim() };
        }

        
        bool AddUnique(HashSet<Guid> set, ProductCardDto p)
        {
            return set.Add(p.Id);
        }
    }

    public async Task<BaseResponse<ProductCardDto>> GetBySKUAsync(string sku)
    {
        var p = await _productRepository.GetAll()
        .Where(x => x.SKU == sku)
        .Include(x => x.Images)
        .FirstOrDefaultAsync();

        if (p is null) return new(_localizer.Get("Products_NotFound"), HttpStatusCode.NotFound);

        var dto = new ProductCardDto(
            p.Id,
            p.SID,
            p.SlugAz,
            p.TitleAz,
            p.TitleRu ?? p.TitleAz,
            p.TitleEn ?? p.TitleAz,
            p.DescAz,
            p.DescRu,
            p.DescEn,
            p.PriceAZN,
            p.PartnerPriceAZN,
            p.Images
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.SortOrder)
                .Select(i => i.Url)
                .FirstOrDefault()
        );

        return new(_localizer.Get("Products_Found"), dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductCardDto>>> GetAllAsync(int size = 40)
    {
        // təhlükəsiz limit
        if (size < 1) size = 1;
        if (size > 60) size = 60;

        var list = await _productRepository.GetAll()
            .Include(p => p.Images)
            .OrderBy(p => Guid.NewGuid())   // SQL Server: ORDER BY NEWID()
            .Take(size)
            .ToListAsync();

        if (list.Count == 0)
            return new(_localizer.Get("Products_NotFound"), HttpStatusCode.NotFound);

        var dtos = list.Select(p =>
        {
            var mainUrl = p.Images?
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.IsMain)
                .ThenBy(i => i.SortOrder)
                .Select(i => i.Url)
                .FirstOrDefault();

            return new ProductCardDto(
                p.Id,
                p.SID,
                p.SlugAz,
                p.TitleAz,                 // TitleAZ
                p.TitleRu ?? p.TitleAz,    // TitleRU (fallback AZ)
                p.TitleEn ?? p.TitleAz,    // TitleEN (fallback AZ)
                p.DescAz,                  // ShortDescAZ
                p.DescRu,                  // ShortDescRU
                p.DescEn,                  // ShortDescEN
                p.PriceAZN,                // PriceUser
                p.PartnerPriceAZN,         // PricePartnor
                mainUrl                    // MainImageUrl
            );
        }).ToList();

        return new(_localizer.Get("Products_Found"), dtos, HttpStatusCode.OK);
    }



    public async Task<BaseResponse<string>> UploadMainImageAsync(ProductMainImageUploadDto dto)
    {
        if (!IsValidImage(dto.File))
            return new(_localizer.Get("Image_File_TypeNotAllowed"), HttpStatusCode.BadRequest);

        var p = await _productRepository.GetByIdFiltered(
            x => x.Id == dto.ProductId,
            include: new Expression<Func<Product, object>>[] { x => x.Images },
            isTracking: true
        ).FirstOrDefaultAsync();

        if (p is null)
            return new(_localizer.Get("Product_NotFound"), HttpStatusCode.NotFound);

        // validator artıq yoxlayır, amma bir də service-də qoruyuruq
        if (p.Images.Any(i => i.IsMain && !i.IsDeleted))
            return new(_localizer.Get("Image_Main_AlreadyExists"), HttpStatusCode.BadRequest);

        var (url, publicId) = await _cloud.UploadImageAsync(dto.File, $"ecommerce/products/{dto.ProductId}");
        if (url is null)
            return new(_localizer.Get("Image_Upload_Failed"), HttpStatusCode.BadRequest);



        var img = new ProductImage
        {
            ProductId = dto.ProductId,
            Url = url,
            PublicId = publicId,
            IsMain = true,
            SortOrder = NextOrder(p.Images),
            AltAz = dto.AltAz,
            AltRu = dto.AltRu,
            AltEn = dto.AltEn
        };

        p.Images.Add(img);
        _productRepository.Update(p);
        await _productRepository.SaveChangeAsync();

        return new(_localizer.Get("Image_Main_Uploaded"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> UploadAdditionalImageAsync(ProductAdditionalImageUploadDto dto)
    {
        if (!IsValidImage(dto.File))
            return new(_localizer.Get("Image_File_TypeNotAllowed"), HttpStatusCode.BadRequest);

        var p = await _productRepository.GetByIdFiltered(
            x => x.Id == dto.ProductId,
            include: new Expression<Func<Product, object>>[] { x => x.Images },
            isTracking: true
        ).FirstOrDefaultAsync();

        if (p is null)
            return new(_localizer.Get("Product_NotFound"), HttpStatusCode.NotFound);

        // limit: ən çox 4 additional
        var additionalCount = p.Images.Count(i => !i.IsDeleted && !i.IsMain);
        if (additionalCount >= 4)
            return new(_localizer.Get("Image_Additional_LimitExceeded"), HttpStatusCode.BadRequest);

        var (url, publicId) = await _cloud.UploadImageAsync(dto.File, $"ecommerce/products/{dto.ProductId}");
        if (url is null)
            return new(_localizer.Get("Image_Upload_Failed"), HttpStatusCode.BadRequest);

        var makeMain = !p.Images.Any(i => i.IsMain && !i.IsDeleted);

        if (makeMain)
            foreach (var i in p.Images.Where(i => i.IsMain && !i.IsDeleted))
                i.IsMain = false;

        var img = new ProductImage
        {
            ProductId = dto.ProductId,
            Url = url,
            PublicId = publicId,
            IsMain = makeMain,
            SortOrder = NextOrder(p.Images),
            AltAz = dto.AltAz,
            AltRu = dto.AltRu,
            AltEn = dto.AltEn
        };

        p.Images.Add(img);
        _productRepository.Update(p);
        await _productRepository.SaveChangeAsync();

        return new(_localizer.Get(makeMain ? "Image_Main_Uploaded" : "Image_Added"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> RemoveImageAsync(Guid imageId)
    {


        var img = await _productImageRepository.GetAll().FirstOrDefaultAsync(i => i.Id == imageId && !i.IsDeleted);
        if (img is null)
            return new(_localizer.Get("Image_NotFound"), HttpStatusCode.NotFound);



        // əvvəl Cloudinary
        var ok = await _cloud.DeleteImageAsync(img.PublicId);
        if (!ok)
            return new(_localizer.Get("Cloud_Delete_Failed"), HttpStatusCode.BadRequest);

        // sonra DB
        _productImageRepository.HardDelete(img);
        await _productImageRepository.SaveChangeAsync();

        return new(_localizer.Get("Image_Deleted"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> SetMainImageAsync(SetMainDto dto)
    {
        var p = await _productRepository.GetByIdFiltered(
       x => x.Id == dto.productId,
       include: new Expression<Func<Product, object>>[] { x => x.Images },
       isTracking: true
   ).FirstOrDefaultAsync();

        if (p is null)
            return new(_localizer.Get("Product_NotFound"), HttpStatusCode.NotFound);

        var img = p.Images.FirstOrDefault(i => i.Id == dto.imageId && !i.IsDeleted);
        if (img is null)
            return new(_localizer.Get("Image_NotFound"), HttpStatusCode.NotFound);

        foreach (var i in p.Images.Where(i => !i.IsDeleted))
            i.IsMain = false;

        img.IsMain = true;

        _productRepository.Update(p);
        await _productRepository.SaveChangeAsync();

        return new(_localizer.Get("Image_SetAsMain"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> ReorderImagesAsync(ListedReorderDto dto)
    {

        if (dto.items == null || dto.items.Count == 0)
            return new(_localizer.Get("Nothing_To_Update"), HttpStatusCode.BadRequest);

        var map = dto.items.ToDictionary(i => i.ImageId, i => i.SortOrder);

        var images = await _productImageRepository.GetAll(isTracking: true)
            .Where(i => i.ProductId == dto.productId && map.Keys.Contains(i.Id))
            .ToListAsync();

        if (images.Count == 0)
            return new(_localizer.Get("Image_NotFound"), HttpStatusCode.NotFound);

        foreach (var i in images.Where(x => !x.IsDeleted))
            i.SortOrder = map[i.Id];

        await _productImageRepository.SaveChangeAsync();
        return new(_localizer.Get("Images_Reordered"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> UpdateImageAltAsync(ProductUpdateAltDto dto)
    {
        var img = await _productImageRepository.GetByIdFiltered(
        x => x.Id == dto.imageId,
        include: new Expression<Func<ProductImage, object>>[] { x => x.Product },
        isTracking: true
    ).FirstOrDefaultAsync();

        if (img is null || img.IsDeleted)
            return new(_localizer.Get("Image_NotFound"), HttpStatusCode.NotFound);

       
            img.AltAz = dto.altAz;
            img.AltRu = dto.altRu;
            img.AltEn = dto.altEn;
        

        _productImageRepository.Update(img);
        await _productImageRepository.SaveChangeAsync();

        return new(_localizer.Get("Image_Alt_Updated"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<ProductImageDto>>> GetImagesAsync(Guid productId)
    {
        var images = await _productImageRepository.GetAll()
        .Where(i => i.ProductId == productId && !i.IsDeleted)
        .OrderByDescending(i => i.IsMain)
        .ThenBy(i => i.SortOrder)
        .Select(i => new ProductImageDto(
            i.Id,
            i.Url,
            i.IsMain,
            i.SortOrder,
            i.AltAz,
            i.AltEn,
            i.AltRu
        ))
        .ToListAsync();

        if (images.Count == 0)
            return new(_localizer.Get("Image_NotFound"), HttpStatusCode.NotFound);

        return new(_localizer.Get("Images_Found"), images, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<ProductImageDto>> GetMainImageAsync(Guid productId)
    {
        var dto = await _productImageRepository.GetAll()
         .Where(i => i.ProductId == productId && !i.IsDeleted)
         .OrderByDescending(i => i.IsMain)
         .ThenBy(i => i.SortOrder)
         .Select(i => new ProductImageDto(
             i.Id,
             i.Url,
             i.IsMain,
             i.SortOrder,
             i.AltAz,
             i.AltEn,
             i.AltRu
         ))
         .FirstOrDefaultAsync();

        if (dto is null)
            return new(_localizer.Get("Image_NotFound"), HttpStatusCode.NotFound);

        return new(_localizer.Get("Image_Main_Found"), dto, HttpStatusCode.OK);
    }






    private static bool IsValidImage(IFormFile file, long maxBytes = 5 * 1024 * 1024)
    {
        if (file == null || file.Length == 0) return false;
        if (file.Length > maxBytes) return false;

        var ct = file.ContentType?.ToLowerInvariant();
        var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();

        bool okType = ct == "image/jpeg" || ct == "image/jpg" || ct == "image/png";
        bool okExt = ext == ".jpg" || ext == ".jpeg" || ext == ".png";
        return okType || okExt;
    }
    private static int NextOrder(IEnumerable<ProductImage> images)
    {
        var active = images.Where(i => !i.IsDeleted);
        return (active.Any() ? active.Max(i => i.SortOrder) : 0) + 10;
    }
    private static string GetLocalizedName(string nameAz, string? nameRu, string? nameEn)
    {
        var culture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        return culture switch
        {
            "az" => nameAz,
            "ru" => nameRu ?? nameAz,
            "en" => nameEn ?? nameAz,
            _ => nameAz
        };
    }
    private async Task<string> GenerateUniqueSidAsync(int len = 8)
    {
        // Base36 (0-9a-z) random qısa id — unikallıq üçün loop
        const string alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";
        while (true)
        {
            var bytes = RandomNumberGenerator.GetBytes(len);
            var chars = new char[len];
            for (int i = 0; i < len; i++)
                chars[i] = alphabet[bytes[i] % alphabet.Length];

            var sid = new string(chars);
            var exists = await _productRepository.AnyAsync(p => p.SID == sid);
            if (!exists) return sid;
            // nadir hallarda toqquşma olsa təkrar cəhd
        }
    }
    private string NormalizeSlug(string input)
    {
        input ??= "";
        input = input.Trim().ToLowerInvariant();

        // AZ translit
        input = input
            .Replace("ə", "e").Replace("ö", "o").Replace("ü", "u")
            .Replace("ğ", "g").Replace("ı", "i").Replace("ş", "s").Replace("ç", "c");

        input = Regex.Replace(input, @"[^a-z0-9\s-]", "");
        input = Regex.Replace(input, @"\s+", "-").Trim('-');
        return input;
    }
    private async Task<string> MakeUniqueProductSlugAsync(string baseSlug, Guid? excludeId = null)
    {
        var slug = baseSlug; var i = 2;
        while (await _productRepository.AnyAsync(p => p.SlugAz == slug && p.Id != excludeId))
            slug = $"{baseSlug}-{i++}";
        return slug;
    }

    private List<(string Name, string Slug)> NormalizeTags(IEnumerable<string>? raw)
    {
        var list = new List<(string Name, string Slug)>();
        if (raw == null) return list;

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var t in raw)
        {
            var name = (t ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name)) continue;

            var slug = NormalizeSlug(name);
            if (string.IsNullOrWhiteSpace(slug)) continue;

            // dublikatları slug-a görə atırıq
            if (!seen.Add(slug)) continue;

            list.Add((name, slug));
        }
        return list;
    }

    private async Task AttachTagsAsync(Product p, List<(string Name, string Slug)> tags)
    {
        if (tags.Count == 0) return;

        p.ProductTags ??= new List<ProductTag>();

        // Mövcud tag-ları slug-a görə lüğətə yığaq (case-insensitive)
        var wantedSlugs = tags.Select(t => t.Slug).ToList();
        var existing = await _tagRepository.GetAll(isTracking: true)
            .Where(t => wantedSlugs.Contains(t.Slug))
            .ToDictionaryAsync(t => t.Slug, StringComparer.OrdinalIgnoreCase);

        foreach (var t in tags)
        {
            if (!existing.TryGetValue(t.Slug, out var tag))
            {
                tag = new Tag { Name = t.Name, Slug = t.Slug };
                await _tagRepository.AddAsync(tag);
                existing[t.Slug] = tag;
            }

            // dublikat join yazmamaq üçün ehtiyat yoxlama (eyni slug iki dəfə gəlməyib, amma yenə də…)
            if (!p.ProductTags.Any(pt => pt.TagId == tag.Id || pt.Tag == tag))
                p.ProductTags.Add(new ProductTag { Product = p, Tag = tag });
        }
    }

    private async Task ReplaceTagsAsync(Product p, List<(string Name, string Slug)> desired)
    {
        // hazırkı tag-ları yüklə (id-lər lazımdır)
        await _productRepository.GetAll(isTracking: true)
            .Where(x => x.Id == p.Id)
            .Select(x => x.ProductTags.Select(pt => pt.Tag.Slug).ToList())
            .FirstOrDefaultAsync(); // kontekstə track olunur

        // detach: köhnə join-ları təmizlə
        p.ProductTags.Clear();

        // yenilərini əlavə et
        await AttachTagsAsync(p, desired);
    }

}
