using Volo.Abp.Modularity;

namespace csuwf.PaperManagement;

[DependsOn(
    typeof(PaperManagementApplicationContractsModule)

    )]
    public class PaperManagementHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }

}
