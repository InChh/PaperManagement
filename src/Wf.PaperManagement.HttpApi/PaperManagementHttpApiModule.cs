using Volo.Abp.Modularity;

namespace Wf.PaperManagement;

[DependsOn(
    typeof(PaperManagementApplicationContractsModule)

    )]
    public class PaperManagementHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }

}
