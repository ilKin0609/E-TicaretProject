using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret_Project.Persistence.Repositories;

public class ProductRepository:GenericRepository<Product>,IProductRepository
{
    public ProductRepository(E_TicaretProjectDbContext context) : base(context)
    {

    }

    public async Task<Image?> GetImageByIdAsync(Guid id)
    {
        return await _context.Images.FirstOrDefaultAsync(I=> I.Id == id && !I.IsDeleted);
    }
    public void UpdateAsync(Image entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Entry(entity).State = EntityState.Modified;
    }
    public void DeleteAsync(Image entity)
    {
        entity.IsDeleted = true;
        UpdateAsync(entity);
    }
}
