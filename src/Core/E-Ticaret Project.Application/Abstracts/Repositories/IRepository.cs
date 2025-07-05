using E_Ticaret_Project.Domain.Entities;
using System.Linq.Expressions;

namespace E_Ticaret_Project.Application.Abstracts.Repositories;

public interface IRepository<T> where T: BaseEntity, new()
{
    Task<T?> GetByIdAsync(Guid id);

    IQueryable<T> GetByIdFiltered(Expression<Func<T,bool>>? predicate,
        Expression<Func<T, object>>[]? include,
        bool isTracking=false);

    IQueryable<T> GetAll(bool isTracking=false);

    IQueryable<T> GetAllFiltered(Expression<Func<T,bool>>? predicate,
        Expression<Func<T, object>>[]? include,
        Expression<Func<T,object>>? OrderBy,
        bool isOrderBy=true,
        bool isTracking=false);

    Task<int> CountAsync(Expression<Func<T, bool>>? predicate);
    Task<bool> AnyAsync(Expression<Func<Favorite, bool>> predicate);

    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangeAsync();
}
