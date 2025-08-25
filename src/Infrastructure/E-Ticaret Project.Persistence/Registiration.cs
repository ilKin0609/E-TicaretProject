using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Infrastructure.Services;
using E_Ticaret_Project.Persistence.Repositories;
using E_Ticaret_Project.Persistence.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace E_Ticaret_Project.Persistence;

public static class Registiration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISpecialRequestRepository, SpecialRequestRepository>();
        services.AddScoped<IAboutRepository, AboutRepository>();
        services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
        services.AddScoped<IKeywordSearchStatRepository, KeywordSearchStatRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
        services.AddScoped<IInstaLinkRepository, InstaLinkRepository>();
        #endregion


        #region Servicies
        
        
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICloudinaryService,CloudinaryService>();
        services.AddScoped<IBackgroundJobService, HangfireJobService>();
        services.AddSingleton<ILocalizationService, LocalizationService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAboutService, AboutService>();
        services.AddScoped<IContactInfoService, ContactInfoService>();
        services.AddScoped<ISpecialRequestService, SpecialRequestService>();
        services.AddScoped<ISiteSettingService, SiteSettingService>();
        services.AddScoped<IInstaLinkService, InstaLinkService>();



        #endregion
    }
}
