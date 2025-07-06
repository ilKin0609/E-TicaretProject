using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Infrastructure.Services;
using E_Ticaret_Project.Persistence.Repositories;
using E_Ticaret_Project.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_Ticaret_Project.Persistence;

public static class Registiration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IReviewAndCommentRepository, ReviewAndCommentRepository>();
        #endregion


        #region Servicies
        
        
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<IReviewAndCommentService, ReviewAndCommentService>();
        services.AddScoped<IProductService, ProductService>();

        #endregion
    }
}
