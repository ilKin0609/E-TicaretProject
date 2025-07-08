using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Ticaret_Project.Persistence.Repositories;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity, new()
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
        entity.IsDeleted = true;
        Update(entity);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await Table.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public IQueryable<T> GetAll(bool isTracking = false)
    {
        IQueryable<T> query = Table.Where(e => !e.IsDeleted);

        if (!isTracking)
            query = query.AsNoTracking();

        return query;
    }

    public IQueryable<T> GetByIdFiltered(Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        bool isTracking = false)
    {
        IQueryable<T> query = Table.Where(e => !e.IsDeleted);

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
        {
            foreach (var IncProp in include)
            {
                query = query.Include(IncProp);
            }
        }

        if (!isTracking)
            query = query.AsNoTracking();

        return query;
    }

    public IQueryable<T> GetAllFiltered(Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        Expression<Func<T, object>>? OrderBy = null,
        bool isOrderBy = true,
        bool isTracking = false)
    {
        IQueryable<T> query = Table.Where(e => !e.IsDeleted);

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
        {
            foreach (var IncProp in include)
            {
                query = query.Include(IncProp);
            }
        }

        if (OrderBy is not null)
        {
            if (isOrderBy)
                query = query.OrderBy(OrderBy);

            else
                query = query.OrderByDescending(OrderBy);
        }

        if (!isTracking)
            query = query.AsNoTracking();

        return query;
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = Table.Where(e => !e.IsDeleted);

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.CountAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = Table.Where(e => !e.IsDeleted);

        query = query.Where(predicate);

        return await query.AnyAsync();
    }

    public void HardDelete(T entity)
    {
        Table.Remove(entity);
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}
