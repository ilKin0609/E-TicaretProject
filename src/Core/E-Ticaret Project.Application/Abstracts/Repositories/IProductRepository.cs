using E_Ticaret_Project.Domain.Entities;

namespace E_Ticaret_Project.Application.Abstracts.Repositories;

public interface IProductRepository:IRepository<Product>
{
    Task<Image?> GetImageByIdAsync(Guid id);
    void DeleteAsync(Image entity);
    void UpdateAsync(Image entity);
}
