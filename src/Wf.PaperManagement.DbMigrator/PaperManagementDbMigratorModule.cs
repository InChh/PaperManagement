using csuwf.PaperManagement.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace csuwf.PaperManagement.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(PaperManagementEntityFrameworkCoreModule),
    typeof(PaperManagementApplicationContractsModule)
    )]
public class PaperManagementDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "PaperManagement:"; });
    }
}
