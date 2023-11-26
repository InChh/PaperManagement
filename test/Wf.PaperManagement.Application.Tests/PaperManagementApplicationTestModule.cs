using Volo.Abp.Modularity;

namespace Wf.PaperManagement;

[DependsOn(
    typeof(PaperManagementApplicationModule),
    typeof(PaperManagementDomainTestModule)
    )]
public class PaperManagementApplicationTestModule : AbpModule
{

}
