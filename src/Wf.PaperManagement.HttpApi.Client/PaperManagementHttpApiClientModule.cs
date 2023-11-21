using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;

using Volo.Abp.VirtualFileSystem;

namespace csuwf.PaperManagement;

[DependsOn(
    typeof(PaperManagementApplicationContractsModule)
)]
    public class PaperManagementHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(PaperManagementApplicationContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PaperManagementHttpApiClientModule>();
        });
    }
}
