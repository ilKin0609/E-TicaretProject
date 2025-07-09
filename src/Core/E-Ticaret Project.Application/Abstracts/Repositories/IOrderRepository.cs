using E_Ticaret_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Ticaret_Project.Application.Abstracts.Repositories;

public interface IOrderRepository:IRepository<Order>
{
    IQueryable<Order> GetAllOrderFiltered(
        Expression<Func<Order, bool>>? predicate = null,
        Func<IQueryable<Order>, IQueryable<Order>>? include = null,
        Expression<Func<Order, object>>? orderBy = null,
        bool isOrderBy = true,
        bool isTracking = false);

    IQueryable<Order> GetOrderByIdFiltered(
    Expression<Func<Order, bool>>? predicate = null,
    Func<IQueryable<Order>, IQueryable<Order>>? include = null,
    bool isTracking = false);
}
