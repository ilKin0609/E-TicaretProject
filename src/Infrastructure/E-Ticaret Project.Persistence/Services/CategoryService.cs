using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.CategoryDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_Ticaret_Project.Persistence.Services;

public class CategoryService : ICategoryService
{
    private ICategoryRepository _categoryRepository { get; }
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<BaseResponse<string>> CreateAsync(CategoryCreateDto dto)
    {
        Category? parentCategory = null;

        if (dto.ParentCategoryId is not null)
        {
            parentCategory = await _categoryRepository.GetByIdAsync(dto.ParentCategoryId.Value);
            if (parentCategory is null)
                return new("Parent category not found", HttpStatusCode.NotFound);
        }

        var newCategory = new Category
        {
            Name = dto.Name,
            ParentCategoryId = parentCategory?.Id,
            ParentCategory = parentCategory
        };

        await _categoryRepository.AddAsync(newCategory);
        await _categoryRepository.SaveChangeAsync();

        return new("Category created successfully", HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string>> DeleteAsync(Guid id)
    {
        var allCategories = await _categoryRepository.GetAll().ToListAsync();
        var target = allCategories.FirstOrDefault(c => c.Id == id);

        if (target is null)
            return new("Category not found", HttpStatusCode.NotFound);

        var visited = new HashSet<Guid>();

        void DeleteWithChildren(Guid parentId)
        {
            if (!visited.Add(parentId))
                return;

            var children = allCategories.Where(c => c.ParentCategoryId == parentId).ToList();
            foreach (var child in children)
                DeleteWithChildren(child.Id);

            var toDelete = allCategories.First(c => c.Id == parentId);
            _categoryRepository.Delete(toDelete);
        }

        DeleteWithChildren(id);
        await _categoryRepository.SaveChangeAsync();

        return new("Category and related subcategories deleted", HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> UpdateAsync(CategoryUpdateDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.Id);
        if (category is null)
            return new("Category not found", HttpStatusCode.NotFound);

        if (dto.ParentCategoryId == dto.Id)
            return new("A category cannot be its own parent", HttpStatusCode.BadRequest);

        if (dto.ParentCategoryId is not null)
        {
            var parentCategory = await _categoryRepository.GetByIdAsync(dto.ParentCategoryId.Value);
            if (parentCategory is null)
                return new("Parent category not found", HttpStatusCode.NotFound);
        }

        category.Name = dto.Name;
        category.ParentCategoryId = dto.ParentCategoryId;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangeAsync();

        return new("Category updated successfully", HttpStatusCode.OK);
    }
    public async Task<BaseResponse<List<CategoryGetDto>>> GetAllRecursiveAsync()
    {
        var allCategories = await _categoryRepository.GetAll().ToListAsync();
        var rootCategories = allCategories.Where(c => c.ParentCategoryId == null).ToList();

        var result = new List<CategoryGetDto>();
        foreach (var category in rootCategories)
        {
            result.Add(new CategoryGetDto(
                Id: category.Id,
                Name: category.Name,
                ParentCategoryId: category.ParentCategoryId,
                ParentCategoryName: null,
                SubCategories: GetSubCategoryDtos(allCategories, category.Id)
            ));
        }

        return new("All categories fetched", result, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<CategoryGetDto>> GetByIdAsync(Guid id)
    {
        var allCategories = await _categoryRepository.GetAll().ToListAsync();
        var category = allCategories.FirstOrDefault(c => c.Id == id);

        if (category is null)
            return new("Category not found", HttpStatusCode.NotFound);
            
        var result = new CategoryGetDto(
            Id:id,
            Name: category.Name,
            ParentCategoryId: category.ParentCategoryId,
            ParentCategoryName: category.ParentCategory?.Name,
            SubCategories:GetSubCategoryDtos(allCategories,category.Id)
            );
        return new("Category found", result, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<CategoryGetDto>>> GetByNameAsync(string name)
    {
        var categories = await _categoryRepository
            .GetAllFiltered(c => c.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();

        if (!categories.Any())
            return new("No category found with this name.", HttpStatusCode.NotFound);

        var allCategories = await _categoryRepository.GetAll().ToListAsync();
        var result = categories.Select(category => new CategoryGetDto(
            Id: category.Id,
            Name: category.Name,
            ParentCategoryId: category.ParentCategoryId,
            ParentCategoryName: category.ParentCategory?.Name,
            SubCategories: GetSubCategoryDtos(allCategories, category.Id)
        )).ToList();

        return new("Categories found", result, HttpStatusCode.OK);
    }


    private List<SubCategoryDto> GetSubCategoryDtos(List<Category> allCategories, Guid parentId)
    {
        var children = allCategories
            .Where(c => c.ParentCategoryId == parentId)
            .Select(c => new SubCategoryDto(Id: c.Id, Name: c.Name))
            .ToList();
        return children;
    }

}
