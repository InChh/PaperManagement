using csuwf.PaperManagement.Localization;
using Volo.Abp.Application.Services;

namespace csuwf.PaperManagement;

/* Inherit your application services from this class.
 */
public abstract class PaperManagementAppService : ApplicationService
{
    protected PaperManagementAppService()
    {
        LocalizationResource = typeof(PaperManagementResource);
    }
}
