using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Domain.Entities;
using E_Ticaret_Project.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace E_Ticaret_Project.Persistence.Repositories;

public class OrderRepository:GenericRepository<Order>,IOrderRepository
{
    public OrderRepository(E_TicaretProjectDbContext context) : base(context)
    {

    }

    public IQueryable<Order> GetAllOrderFiltered(
    Expression<Func<Order, bool>>? predicate = null,
    Func<IQueryable<Order>, IQueryable<Order>>? include = null,
    Expression<Func<Order, object>>? orderBy = null,
    bool isOrderBy = true,
    bool isTracking = false)
    {
        IQueryable<Order> query = Table.Where(e => !e.IsDeleted);

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
            query = include(query); 

        if (orderBy is not null)
        {
            query = isOrderBy ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        if (!isTracking)
            query = query.AsNoTracking();

        return query;
    }

    public IQueryable<Order> GetOrderByIdFiltered(
    Expression<Func<Order, bool>>? predicate = null,
    Func<IQueryable<Order>, IQueryable<Order>>? include = null,
    bool isTracking = false)
    {
        IQueryable<Order> query = Table.Where(e => !e.IsDeleted);

        if (predicate is not null)
            query = query.Where(predicate);

        if (include is not null)
            query = include(query);

        if (!isTracking)
            query = query.AsNoTracking();

        return query;
    }
}
