using E_Ticaret_Project.Application.Abstracts.Repositories;
using E_Ticaret_Project.Persistence.Contexts;

namespace E_Ticaret_Project.Persistence.Repositories;

public class SiteSettingRepository:GenericRepository<SiteSetting>,ISiteSettingRepository
{
    public SiteSettingRepository(E_TicaretProjectDbContext context):base(context)
    {
        
    }
}
