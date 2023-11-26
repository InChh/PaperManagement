using Wf.PaperManagement.Localization;
using Volo.Abp.Application.Services;

namespace Wf.PaperManagement;

/* Inherit your application services from this class.
 */
public abstract class PaperManagementAppService : ApplicationService
{
    protected PaperManagementAppService()
    {
        LocalizationResource = typeof(PaperManagementResource);
    }
}
