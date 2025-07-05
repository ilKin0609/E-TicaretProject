using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Ticaret_Project.Persistence.Repositories;

public class GenericRepository<T>:IRepository<T> where T : BaseEntity, new()
{
    protected readonly E_TicaretProjectDbContext _context;
    protected readonly DbSet<T> Table;

    public GenericRepository(E_TicaretProjectDbContext context)
    {
        _context = context;
        Table = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.Now;
        await Table.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        Table.Remove(entity);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await Table.FindAsync(id);
    }

    public IQueryable<T> GetAll(bool isTracking = false)
    {
        IQueryable<T> query = Table;

        if(!isTracking)
            query=query.AsNoTracking();

        return query;
    }

    public IQueryable<T> GetByIdFiltered(Expression<Func<T, bool>>? predicate,
        Expression<Func<T, object>>[]? include,
        bool isTracking = false)
    {
        IQueryable<T> query = Table;

        if(predicate is not null)
            query = query.Where(predicate);

        if(include is not null)
        {
            foreach (var IncProp in include)
            {
                query = query.Include(IncProp);
            }
        }

        if(!isTracking)
            query = query.AsNoTracking();

        return query;
    }

    public IQueryable<T> GetAllFiltered(Expression<Func<T, bool>>? predicate,
        Expression<Func<T, object>>[]? include,
        Expression<Func<T, object>>? OrderBy,
        bool isOrderBy = true,
        bool isTracking = false)
    {
        IQueryable<T> query = Table;

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
        {
            foreach (var IncProp in include)
            {
                query = query.Include(IncProp);
            }
        }

        if(OrderBy is not null)
        {
            if(isOrderBy)
                query = query.OrderBy(OrderBy);

            else
                query = query.OrderByDescending(OrderBy);
        }

        if (!isTracking)
            query = query.AsNoTracking();

        return query;
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate)
    {
        
        if(predicate is not null)
            return await Table.CountAsync(predicate);

        return await Table.CountAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<Favorite, bool>> predicate)
    {
        return await _context.Favorites.AnyAsync(predicate);
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}
