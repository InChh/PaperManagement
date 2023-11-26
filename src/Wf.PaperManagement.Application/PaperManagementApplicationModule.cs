using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;


namespace Wf.PaperManagement;

[DependsOn(
    typeof(PaperManagementDomainModule),
    typeof(PaperManagementApplicationContractsModule)
)]
public class PaperManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<PaperManagementApplicationAutoMapperProfile>(validate: true);
        });
    }
}