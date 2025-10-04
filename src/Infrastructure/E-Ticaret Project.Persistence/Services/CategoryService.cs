using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;

namespace E_Ticaret_Project.Persistence.Services;

public class CategoryService : ICategoryService
{
    private ICategoryRepository _categoryRepository { get; }
    private ILocalizationService _localizer { get; }
    private IKeywordSearchStatRepository _keywordStatRepo { get; }
    private ITagRepository _tagRepo { get; }
    
    public CategoryService(ICategoryRepository categoryRepository,
        ILocalizationService localizer,
        IKeywordSearchStatRepository keywordStatRepo,
        ITagRepository tagRepo)
    {
        _categoryRepository = categoryRepository;
        _localizer = localizer;
        _keywordStatRepo = keywordStatRepo;
        _tagRepo = tagRepo;
    }
    public async Task<BaseResponse<string>> CreateAsync(CategoryCreateDto dto)
    {
        Category? parentCategory = null;

        if (dto.ParentCategoryId is not null)
        {
            parentCategory = await _categoryRepository.GetByIdAsync(dto.ParentCategoryId.Value);
            if (parentCategory is null)
                return new(_localizer.Get("Parent_category_not_found"), HttpStatusCode.NotFound);
        }
        var dup = await _categoryRepository.AnyAsync(
    x => x.ParentCategoryId == dto.ParentCategoryId
      && (x.NameAz == dto.NameAz || x.NameRu == dto.NameRu || x.NameEn == dto.NameEn)
);
        if (dup) return new(_localizer.Get("Category_Name_AlreadyExists"), HttpStatusCode.BadRequest);

        var slugBase = NormalizeSlug(dto.NameAz);
        var uniqueSlug = await MakeUniqueSlugAsync(slugBase);

        var newCategory = new Category
        {
            NameAz = dto.NameAz,
            NameRu = dto.NameRu,
            NameEn = dto.NameEn,
            ParentCategoryId = parentCategory?.Id,
            Slug = uniqueSlug,
            IsVisible = true,
            Order = await GetNextOrderAsync(dto.ParentCategoryId),
            MetaTitleAz = dto.MetaTitleAz,
            MetaTitleRu = dto.MetaTitleRu,
            MetaTitleEn = dto.MetaTitleEn,
            MetaDescriptionAz = dto.MetaDescriptionAz,
            MetaDescriptionRu = dto.MetaDescriptionRu,
            MetaDescriptionEn = dto.MetaDescriptionEn,
            Keywords = dto.Keywords
        };

        await _categoryRepository.AddAsync(newCategory);
        await _categoryRepository.SaveChangeAsync();

        return new(_localizer.Get("Category_Created"), true, HttpStatusCode.Created);
    }
    public async Task<BaseResponse<string>> DeleteAsync(Guid id)
    {
        var target = await _categoryRepository.GetByIdFiltered(
            x => x.Id == id,
            include: [x => x.SubCategories, x => x.Products],
            isTracking: true
        ).FirstOrDefaultAsync();

        if (target is null)
            return new(_localizer.Get("Category_not_found"), HttpStatusCode.NotFound);

        if (target.SubCategories.Any())
            return new(_localizer.Get("Category_Has_Children_Restrict"), HttpStatusCode.BadRequest);

        if (target.Products.Any())
            return new(_localizer.Get("Category_Has_Products_Restrict"), HttpStatusCode.BadRequest);

        _categoryRepository.Delete(target);
        await _categoryRepository.SaveChangeAsync();

        return new(_localizer.Get("Category_Deleted"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> UpdateAsync(CategoryUpdateDto dto)
    {
        var c = await _categoryRepository.GetByIdAsync(dto.Id);
        if (c is null)
            return new(_localizer.Get("Category_not_found"), HttpStatusCode.NotFound);

        // Guard: heç bir sahə göndərilməyibsə
        bool any =
            dto.NameAz != null || dto.NameRu != null || dto.NameEn != null ||
            dto.ParentCategoryId.HasValue ||
            dto.IsVisible.HasValue || dto.Order.HasValue ||
            dto.MetaTitleAz != null || dto.MetaTitleRu != null || dto.MetaTitleEn != null ||
            dto.MetaDescriptionAz != null || dto.MetaDescriptionRu != null || dto.MetaDescriptionEn != null ||
            dto.Keywords != null;

        if (!any)
            return new(_localizer.Get("Category_Update_AtLeastOneFieldRequired"), HttpStatusCode.BadRequest);

        // Parent dəyişdirilirsə, yoxlamalar
        if (dto.ParentCategoryId.HasValue && dto.ParentCategoryId != c.ParentCategoryId)
        {
            // öz alt qoluna köçməsin
            var loop = await IsDescendantAsync(dto.ParentCategoryId.Value, c.Id);
            if (loop)
                return new(_localizer.Get("Category_Cannot_Move_Under_Descendant"), HttpStatusCode.BadRequest);

            // parent mövcud olmalıdır
            var parentExists = await _categoryRepository.AnyAsync(x => x.Id == dto.ParentCategoryId.Value);
            if (!parentExists)
                return new(_localizer.Get("Parent_category_not_found"), HttpStatusCode.NotFound);
        }

        // Dublikat adı eyni parent altında blokla (ad və ya parent dəyişirsə)
        if (dto.NameAz != null || dto.NameRu != null || dto.NameEn != null || dto.ParentCategoryId.HasValue)
        {
            var targetParentId = dto.ParentCategoryId.HasValue ? dto.ParentCategoryId : c.ParentCategoryId;
            var newAz = dto.NameAz ?? c.NameAz;
            var newRu = dto.NameRu ?? c.NameRu;
            var newEn = dto.NameEn ?? c.NameEn;

            var dup = await _categoryRepository.AnyAsync(x =>
                x.Id != c.Id &&
                x.ParentCategoryId == targetParentId &&
                (x.NameAz == newAz || x.NameRu == newRu || x.NameEn == newEn)
            );
            if (dup)
                return new(_localizer.Get("Category_Name_AlreadyExists"), HttpStatusCode.BadRequest);
        }

        // PATCH üslubu ilə təyin et
        if (dto.NameAz != null) c.NameAz = dto.NameAz;
        if (dto.NameRu != null) c.NameRu = dto.NameRu;
        if (dto.NameEn != null) c.NameEn = dto.NameEn;

        if (dto.ParentCategoryId.HasValue) c.ParentCategoryId = dto.ParentCategoryId.Value; // Qeyd: parent-i null etmək istəyirsənsə, ChangeParent endpointindən istifadə et.

        if (dto.IsVisible.HasValue) c.IsVisible = dto.IsVisible.Value;
        if (dto.Order.HasValue) c.Order = dto.Order.Value;

        if (dto.MetaTitleAz != null) c.MetaTitleAz = dto.MetaTitleAz;
        if (dto.MetaTitleRu != null) c.MetaTitleRu = dto.MetaTitleRu;
        if (dto.MetaTitleEn != null) c.MetaTitleEn = dto.MetaTitleEn;

        if (dto.MetaDescriptionAz != null) c.MetaDescriptionAz = dto.MetaDescriptionAz;
        if (dto.MetaDescriptionRu != null) c.MetaDescriptionRu = dto.MetaDescriptionRu;
        if (dto.MetaDescriptionEn != null) c.MetaDescriptionEn = dto.MetaDescriptionEn;

        if (dto.Keywords != null) c.Keywords = dto.Keywords;

        _categoryRepository.Update(c);
        await _categoryRepository.SaveChangeAsync();

        return new(_localizer.Get("Category_updated_successfully"), true, HttpStatusCode.OK);
    }



    public async Task<BaseResponse<CategoryGetDto>> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdFiltered(
        x => x.Id == id && x.IsVisible,
         include: [x => x.ParentCategory, x => x.SubCategories ]
    ).FirstOrDefaultAsync();

        if (category is null)
            return new(_localizer.Get("Category_not_found"), HttpStatusCode.NotFound);

        var subs = (category.SubCategories ?? new List<Category>())
        .Where(s => s.IsVisible)
        .OrderBy(s => s.Order)
        .Select(s => new SubCategoryDto(
            s.Id, s.NameAz, s.NameRu, s.NameEn, s.Slug,
            s.MetaTitleAz, s.MetaTitleRu, s.MetaTitleEn,
            s.MetaDescriptionAz, s.MetaDescriptionRu, s.MetaDescriptionEn,
            s.Keywords,
            null

        )).ToList();

        var dto = new CategoryGetDto(
            category.Id, category.NameAz, category.NameRu, category.NameEn,
            category.Slug,
            category.MetaTitleAz, category.MetaTitleRu, category.MetaTitleEn,
            category.MetaDescriptionAz, category.MetaDescriptionRu, category.MetaDescriptionEn,
            category.Keywords,
            category.ParentCategoryId,
            category.ParentCategory?.NameAz,
            subs
        );
        return new(_localizer.Get("Category_found"), dto, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<CategoryGetDto>>> GetByNameAsync(string name)
    {
        name = name?.Trim() ?? "";

        var list = await _categoryRepository.GetAllFiltered(
            x =>
                x.IsVisible && // yalnız görünən əsas kateqoriyalar
                (
                    x.NameAz.Contains(name) ||
                    x.NameRu.Contains(name) ||
                    x.NameEn.Contains(name)
                ),
                [
                    x => x.ParentCategory,
                    x => x.SubCategories
                ],
                OrderBy: x => x.Order
        ).ToListAsync();

        if (!list.Any())
            return new(_localizer.Get("Category_not_found"), HttpStatusCode.NotFound);

        var dtos = list.Select(c => new CategoryGetDto(
            c.Id, c.NameAz, c.NameRu, c.NameEn,
            c.Slug,
            c.MetaTitleAz, c.MetaTitleRu, c.MetaTitleEn,
            c.MetaDescriptionAz, c.MetaDescriptionRu, c.MetaDescriptionEn,
            c.Keywords,
            c.ParentCategoryId,
            c.ParentCategory?.NameAz,
            (c.SubCategories ?? new List<Category>())
                .Where(s => s.IsVisible) // yalnız görünən sublar
                .OrderBy(s => s.Order)
                .Select(s => new SubCategoryDto(
                    s.Id, s.NameAz, s.NameRu, s.NameEn, s.Slug,
                    s.MetaTitleAz, s.MetaTitleRu, s.MetaTitleEn,
                    s.MetaDescriptionAz, s.MetaDescriptionRu, s.MetaDescriptionEn,
                    s.Keywords,
                    null
                )).ToList()
        )).ToList();

        return new(_localizer.Get("Category_found"), dtos, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<CategoryGetDto>> GetBySlugAsync(string slug)
    {
        var c = await _categoryRepository.GetAllFiltered(
            x => x.IsVisible && x.Slug == slug,
            [
                x => x.ParentCategory,
                x => x.SubCategories
            ]
        ).FirstOrDefaultAsync();

        if (c == null)
            return new(_localizer.Get("Category_not_found"), HttpStatusCode.NotFound);

        var dto = new CategoryGetDto(
            c.Id, c.NameAz, c.NameRu, c.NameEn,
            c.Slug,
            c.MetaTitleAz, c.MetaTitleRu, c.MetaTitleEn,
            c.MetaDescriptionAz, c.MetaDescriptionRu, c.MetaDescriptionEn,
            c.Keywords,
            c.ParentCategoryId,
            c.ParentCategory?.NameAz,
            (c.SubCategories ?? new List<Category>())
                .Where(s => s.IsVisible) // yalnız görünən sublar
                .OrderBy(s => s.Order)
                .Select(s => new SubCategoryDto(
                    s.Id, s.NameAz, s.NameRu, s.NameEn, s.Slug,
                    s.MetaTitleAz, s.MetaTitleRu, s.MetaTitleEn,
                    s.MetaDescriptionAz, s.MetaDescriptionRu, s.MetaDescriptionEn,
                    s.Keywords,
                    null
                )).ToList()
        );

        return new(_localizer.Get("Category_found"), dto, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<CategoryGetDto>>> GetAllMainCategoriesAsync()
    {
        var mains = await _categoryRepository.GetAllFiltered(
            x => x.ParentCategoryId == null && x.IsVisible,
            OrderBy: x => x.Order
        ).ToListAsync();

        if (!mains.Any())
            return new(_localizer.Get("Category_not_found"), HttpStatusCode.NotFound);

        var dtos = mains.Select(c => new CategoryGetDto(
            c.Id, c.NameAz, c.NameRu, c.NameEn,
            c.Slug,
            c.MetaTitleAz, c.MetaTitleRu, c.MetaTitleEn,
            c.MetaDescriptionAz, c.MetaDescriptionRu, c.MetaDescriptionEn,
            c.Keywords,
            null,
            null,
            null
        )).ToList();

        return new(_localizer.Get("Category_found"), dtos, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<CategoryGetDto>>> GetAllSubCategoriesAsync(Guid parentId)
    {
        var list = await _categoryRepository.GetAllFiltered(
            x => x.ParentCategoryId == parentId && x.IsVisible,
            OrderBy: x => x.Order
        ).ToListAsync();

        if (!list.Any())
            return new(_localizer.Get("Category_not_found"), HttpStatusCode.NotFound);

        var dtos = list.Select(c => new CategoryGetDto(
            c.Id, c.NameAz, c.NameRu, c.NameEn,
            c.Slug,
            c.MetaTitleAz, c.MetaTitleRu, c.MetaTitleEn,
            c.MetaDescriptionAz, c.MetaDescriptionRu, c.MetaDescriptionEn,
            c.Keywords,
            c.ParentCategoryId,
            c.ParentCategory?.NameAz,
            (c.SubCategories ?? new List<Category>())
                .OrderBy(s => s.Order)
                .Select(s => new SubCategoryDto(
                    s.Id, s.NameAz, s.NameRu, s.NameEn, s.Slug,
                    s.MetaTitleAz, s.MetaTitleRu, s.MetaTitleEn,
                    s.MetaDescriptionAz, s.MetaDescriptionRu, s.MetaDescriptionEn,
                    s.Keywords,
                    null
                )).ToList()
        )).ToList();

        return new(_localizer.Get("Category_found"), dtos, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<CategoryGetDto>>> GetAllMainCategoriesAsyncForAdmin()
    {
        var mains = await _categoryRepository.GetAllFiltered(
            x => x.ParentCategoryId == null,
            OrderBy: x => x.Order
        ).ToListAsync();

        if (!mains.Any())
            return new(_localizer.Get("Category_not_found"), HttpStatusCode.NotFound);

        var dtos = mains.Select(c => new CategoryGetDto(
            c.Id, c.NameAz, c.NameRu, c.NameEn,
            c.Slug,
            c.MetaTitleAz, c.MetaTitleRu, c.MetaTitleEn,
            c.MetaDescriptionAz, c.MetaDescriptionRu, c.MetaDescriptionEn,
            c.Keywords,
            null,
            null,
            null
        )).ToList();

        return new(_localizer.Get("Category_found"), dtos, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<CategoryGetDto>>> GetAllSubCategoriesAsyncForAdmin(Guid parentId)
    {
        var list = await _categoryRepository.GetAllFiltered(
            x => x.ParentCategoryId == parentId,
            OrderBy: x => x.Order
        ).ToListAsync();

        if (!list.Any())
            return new(_localizer.Get("Category_not_found"), HttpStatusCode.NotFound);

        var dtos = list.Select(c => new CategoryGetDto(
            c.Id, c.NameAz, c.NameRu, c.NameEn,
            c.Slug,
            c.MetaTitleAz, c.MetaTitleRu, c.MetaTitleEn,
            c.MetaDescriptionAz, c.MetaDescriptionRu, c.MetaDescriptionEn,
            c.Keywords,
            c.ParentCategoryId,
            c.ParentCategory?.NameAz,
            (c.SubCategories ?? new List<Category>())
                .OrderBy(s => s.Order)
                .Select(s => new SubCategoryDto(
                    s.Id, s.NameAz, s.NameRu, s.NameEn, s.Slug,
                    s.MetaTitleAz, s.MetaTitleRu, s.MetaTitleEn,
                    s.MetaDescriptionAz, s.MetaDescriptionRu, s.MetaDescriptionEn,
                    s.Keywords,
                    null
                )).ToList()
        )).ToList();

        return new(_localizer.Get("Category_found"), dtos, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<PopularTagDto>>> GetPopularTagsFromSearchAsync(int take = 8)
    {
        

        var list = await (
        from ks in _keywordStatRepo.GetAll().Where(x => x.Slug != null && x.Slug != "")
        join t in _tagRepo.GetAll() on ks.Slug.ToLower() equals t.Slug.ToLower()
        where t.ProductTags.Any()
        group ks by new { t.Id, t.Name, t.Slug } into g
        orderby g.Sum(x => x.Count) descending, g.Max(x => x.LastSearchedAt) descending
        select new PopularTagDto(
            g.Key.Id,
            g.Key.Name,
            g.Key.Slug,
            g.Sum(x => x.Count)
        )
    )
    .Take(take)
    .ToListAsync();



        if (list.Count == 0)
            return new(_localizer.Get("Category_not_found"), HttpStatusCode.NotFound);

        return new(_localizer.Get("Category_found"), list, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<CategoryGetDto>>> GetAllRecursiveAsync()
    {
        var mains = await _categoryRepository.GetAllFiltered(
          x => x.ParentCategoryId == null,
          OrderBy: x => x.Order
      ).ToListAsync();

        var dtos = new List<CategoryGetDto>(mains.Count); 
        foreach (var r in mains)
        {
            var tree = await BuildChildrenAsync(r.Id);

            dtos.Add(new CategoryGetDto(
                        r.Id, r.NameAz, r.NameRu, r.NameEn,
                        r.Slug,
                        r.MetaTitleAz, r.MetaTitleRu, r.MetaTitleEn,
                        r.MetaDescriptionAz, r.MetaDescriptionRu, r.MetaDescriptionEn,
                        r.Keywords,
                        null,
                        null,
                        tree
                    ));
        }

        return new(_localizer.Get("All_categories_fetched"), dtos, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<CategoryBreadcrumbItemDto>>> GetBreadcrumbAsync(Guid id)
    {
        var chain = new List<CategoryBreadcrumbItemDto>();

        // start node (yalnız görünən)
        var current = await _categoryRepository.GetAll()
            .Where(c => c.Id == id && c.IsVisible)
            .Select(c => new { c.Id, c.Slug, c.NameAz, c.NameEn, c.NameRu, c.ParentCategoryId })
            .FirstOrDefaultAsync();

        if (current is null)
        {
            return new BaseResponse<List<CategoryBreadcrumbItemDto>>(
                _localizer.Get("Category_NotFound"),
                HttpStatusCode.NotFound
            );
        }

        
        while (current != null)
        {
            chain.Add(new CategoryBreadcrumbItemDto(
                current.Id,
               current.NameAz, current.NameEn, current.NameRu,
                current.Slug ?? string.Empty
            ));

            if (current.ParentCategoryId == null) break;

            current = await _categoryRepository.GetAll()
                .Where(c => c.Id == current.ParentCategoryId && c.IsVisible)
                .Select(c => new { c.Id, c.Slug, c.NameAz, c.NameEn, c.NameRu, c.ParentCategoryId })
                .FirstOrDefaultAsync();
        }

        chain.Reverse();

        return new BaseResponse<List<CategoryBreadcrumbItemDto>>(
            _localizer.Get("Breadcrumb_Found"),
            chain,
            HttpStatusCode.OK
        );
    }




    public async Task<BaseResponse<string>> ToggleVisibilityAsync(Guid id, bool isVisible)
    {
        var c = await _categoryRepository.GetByIdAsync(id);
        if (c is null)
            return new BaseResponse<string>(_localizer.Get("Category_NotFound"), HttpStatusCode.NotFound);

        c.IsVisible = isVisible;
        _categoryRepository.Update(c);
        await _categoryRepository.SaveChangeAsync();

        return new BaseResponse<string>(_localizer.Get("Category_Visibility_Updated"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> ReorderAsync(CategoryReorderDto dto)
    {
        var c = await _categoryRepository.GetByIdAsync(dto.Id);
        if (c is null)
            return new BaseResponse<string>(_localizer.Get("Category_NotFound"), HttpStatusCode.NotFound);

        c.Order = dto.NewOrder;
        _categoryRepository.Update(c);
        await _categoryRepository.SaveChangeAsync();

        return new BaseResponse<string>(_localizer.Get("Category_Reorder_Success"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> ReorderBulkAsync(List<CategoryReorderDto> items)
    {
        if (items == null || items.Count == 0)
            return new BaseResponse<string>(_localizer.Get("Nothing_To_Update"), HttpStatusCode.BadRequest);

        var ids = items.Select(i => i.Id).ToHashSet();
        var cats = await _categoryRepository.GetAll(isTracking: true)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        foreach (var it in items)
        {
            var c = cats.FirstOrDefault(x => x.Id == it.Id);
            if (c != null) c.Order = it.NewOrder;
        }

        await _categoryRepository.SaveChangeAsync();
        return new BaseResponse<string>(_localizer.Get("Category_Reorder_Success"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> ChangeParentAsync(Guid id, Guid? newParentId)
    {
        var c = await _categoryRepository.GetByIdAsync(id);
        if (c is null)
            return new BaseResponse<string>(_localizer.Get("Category_NotFound"), HttpStatusCode.NotFound);

        if (newParentId == c.ParentCategoryId)
            return new BaseResponse<string>(_localizer.Get("Category_Parent_Unchanged"), true, HttpStatusCode.OK);

        if (newParentId != null)
        {
            // Döngü yaratmasın: öz alt qolunun altına köçürmək qadağandır
            var loop = await IsDescendantAsync(newParentId.Value, c.Id);
            if (loop)
                return new BaseResponse<string>(_localizer.Get("Category_Cannot_Move_Under_Descendant"), HttpStatusCode.BadRequest);

            var parentExists = await _categoryRepository.AnyAsync(x => x.Id == newParentId);
            if (!parentExists)
                return new BaseResponse<string>(_localizer.Get("Parent_NotFound"), HttpStatusCode.NotFound);
        }

        c.ParentCategoryId = newParentId;
        c.Order = await GetNextOrderAsync(newParentId);
        _categoryRepository.Update(c);
        await _categoryRepository.SaveChangeAsync();

        return new BaseResponse<string>(_localizer.Get("Category_Parent_Changed"), true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<bool>> IsLeafAsync(Guid id)
    {
        var exists = await _categoryRepository.AnyAsync(x => x.Id == id);
        if (!exists)
            return new BaseResponse<bool>(_localizer.Get("Category_NotFound"), HttpStatusCode.NotFound);

        var hasChild = await _categoryRepository.AnyAsync(x => x.ParentCategoryId == id);
        var isLeaf = !hasChild;

        return new BaseResponse<bool>(_localizer.Get("Category_Leaf_Checked"), isLeaf, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> GenerateSlugAsync(string nameAz)
    {
        if (string.IsNullOrWhiteSpace(nameAz))
            return new BaseResponse<string>(_localizer.Get("Slug_Invalid_Name"), HttpStatusCode.BadRequest);

        var baseSlug = NormalizeSlug(nameAz);
        if (string.IsNullOrWhiteSpace(baseSlug))
            return new BaseResponse<string>(_localizer.Get("Slug_Invalid_Name"), HttpStatusCode.BadRequest);

        var unique = await MakeUniqueSlugAsync(baseSlug);
        return new BaseResponse<string>(_localizer.Get("Slug_Generated"), unique, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<string>> IncrementKeywordSearchAsync(string rawKeyword, long delta = 1)
    {
        var name = (rawKeyword ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name))
            return new(_localizer.Get("Keyword_Invalid"), HttpStatusCode.BadRequest);

        var slug = NormalizeSlug(name);
        if (delta < 1) delta = 1;


        var stat = await _keywordStatRepo.GetAll(isTracking: true)
            .FirstOrDefaultAsync(x => x.Slug == slug);

        var now= DateTime.UtcNow;

        if (stat is null)
        {
            stat = new KeywordSearchStat { Keyword = name, Slug = slug, Count = 0 , LastSearchedAt=now};
            await _keywordStatRepo.AddAsync(stat);
        }

        stat.Count += delta;
        stat.LastSearchedAt = now;
        
        await _keywordStatRepo.SaveChangeAsync();

        return new(_localizer.Get("Keyword_Incremented"), true, HttpStatusCode.OK);
    }
    public async Task IncrementKeywordSearchManyAsync(IEnumerable<string> rawKeywords)
    {
        var pairs = rawKeywords
            .Select(k => new { Name = (k ?? "").Trim(), Slug = NormalizeSlug(k) })
            .Where(x => !string.IsNullOrWhiteSpace(x.Slug))
            .GroupBy(x => x.Slug)
            .Select(g => new { Slug = g.Key, Name = g.First().Name })
            .ToList();

        if (pairs.Count == 0) return;

        var slugs = pairs.Select(p => p.Slug).ToList();
        var now = DateTime.UtcNow;

        var existing = await _keywordStatRepo.GetAll(isTracking: true)
            .Where(s => slugs.Contains(s.Slug))
            .ToListAsync();

        foreach (var p in pairs)
        {
            var s = existing.FirstOrDefault(x => x.Slug == p.Slug);
            if (s == null)
            {
                s = new KeywordSearchStat { Keyword = p.Name, Slug = p.Slug, Count = 0, LastSearchedAt = now };
                await _keywordStatRepo.AddAsync(s);
                existing.Add(s);
            }

            s.Count += 1; 
            s.LastSearchedAt = now;
            
        }

        await _keywordStatRepo.SaveChangeAsync();
    }










    //private static string GetLocalizedName(string nameAz, string? nameRu, string? nameEn)
    //{
    //    var culture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

    //    return culture switch
    //    {
    //        "az" => nameAz,
    //        "ru" => nameRu ?? nameAz,
    //        "en" => nameEn ?? nameAz,
    //        _ => nameAz
    //    };
    //}
    private async Task<List<SubCategoryDto>> BuildChildrenAsync(Guid parentId)
    {
        var children = await _categoryRepository.GetAllFiltered(
            x => x.ParentCategoryId == parentId,
            OrderBy: x => x.Order
        ).ToListAsync();

        var result = new List<SubCategoryDto>(children.Count);

        foreach (var ch in children)
        {
            var grandChildren = await BuildChildrenAsync(ch.Id);

            result.Add(new SubCategoryDto(
                ch.Id, ch.NameAz, ch.NameRu, ch.NameEn, ch.Slug,
                ch.MetaTitleAz, ch.MetaTitleRu, ch.MetaTitleEn,
                ch.MetaDescriptionAz, ch.MetaDescriptionRu, ch.MetaDescriptionEn,
                ch.Keywords,
                grandChildren
            ));
        }

        return result;
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
    private async Task<string> MakeUniqueSlugAsync(string baseSlug, Guid? excludeId = null)
    {
        var slug = baseSlug;
        var i = 2;
        while (await _categoryRepository.AnyAsync(x => x.Slug == slug && x.Id != excludeId))
            slug = $"{baseSlug}-{i++}";
        return slug;
    }

    private async Task<int> GetNextOrderAsync(Guid? parentId)
    {
        var max = await _categoryRepository.GetAll()
            .Where(x => x.ParentCategoryId == parentId)
            .Select(x => (int?)x.Order)
            .MaxAsync();
        return (max ?? 0) + 10; // 10-luq addım strategiyası
    }

    private async Task<bool> IsDescendantAsync(Guid id, Guid possibleAncestorId)
    {
        // id node-un ancestor zəncirində possibleAncestorId varmı?
        Guid? current = await _categoryRepository.GetAll()
            .Where(x => x.Id == id)
            .Select(x => x.ParentCategoryId)
            .FirstOrDefaultAsync();

        while (current != null)
        {
            if (current == possibleAncestorId) return true;

            current = await _categoryRepository.GetAll()
                .Where(x => x.Id == current)
                .Select(x => x.ParentCategoryId)
                .FirstOrDefaultAsync();
        }
        return false;
    }


}
