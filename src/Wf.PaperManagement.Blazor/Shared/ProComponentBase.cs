using BlazorComponent.I18n;
using Microsoft.AspNetCore.Components;

namespace Wf.PaperManagement.Blazor.Shared;

public abstract class ProComponentBase : ComponentBase
{
    [Inject]
    protected I18n I18N { get; set; } = null!;
    
    [CascadingParameter(Name = "CultureName")]
    protected string? Culture { get; set; }

    protected string T(string? key, params object[] args)
    {
        return I18N.T(key, args: args);
    }
}
