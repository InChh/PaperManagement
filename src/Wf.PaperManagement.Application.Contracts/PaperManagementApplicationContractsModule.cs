
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;


namespace Wf.PaperManagement;

[DependsOn(
    typeof(PaperManagementDomainSharedModule),
    typeof(AbpObjectExtendingModule)
)]
public class PaperManagementApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PaperManagementDtoExtensions.Configure();
    }
}
