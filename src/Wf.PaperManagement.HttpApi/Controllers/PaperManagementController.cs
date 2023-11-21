using System;
using csuwf.PaperManagement.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace csuwf.PaperManagement.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class PaperManagementController : AbpControllerBase
{
    protected PaperManagementController()
    {
        LocalizationResource = typeof(PaperManagementResource);
    }
}
