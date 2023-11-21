using Volo.Abp.Modularity;

namespace csuwf.PaperManagement;

[DependsOn(
    typeof(PaperManagementApplicationModule),
    typeof(PaperManagementDomainTestModule)
    )]
public class PaperManagementApplicationTestModule : AbpModule
{

}
