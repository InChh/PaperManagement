using System;
using System.Net.Http;
using System.Threading.Tasks;
using csuwf.PaperManagement;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac.WebAssembly;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Wf.PaperManagement.Blazor.Shared;

namespace Wf.PaperManagement.Blazor;

[DependsOn(
    typeof(AbpAutofacWebAssemblyModule),
    typeof(PaperManagementHttpApiClientModule)
)]
public class PaperManagementBlazorModule : AbpModule
{
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var environment = context.Services.GetSingletonInstance<IWebAssemblyHostEnvironment>();
        var builder = context.Services.GetSingletonInstance<WebAssemblyHostBuilder>();

        ConfigureAuthentication(builder);
        builder.Services.AddOptions();
        builder.Services.AddAuthorizationCore();
        ConfigureHttpClient(context, environment);
        await ConfigureMasa(builder);
        ConfigureUI(builder);
    }

    private static async Task ConfigureMasa(WebAssemblyHostBuilder builder)
    {
        await builder.Services.AddMasaBlazor(options =>
        {
            options.ConfigureTheme(theme =>
            {
                theme.Themes.Light.Primary = "#4318FF";
                theme.Themes.Light.Accent = "#4318FF";
            });
        }).AddI18nForWasmAsync($"{builder.Configuration["App:SelfUrl"]!.EnsureEndsWith('/')}i18n");

        await builder.Services.AddGlobalForWasmAsync(builder.Configuration["App:SelfUrl"]!);
    }

    private static void ConfigureAuthentication(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddOidcAuthentication(options =>
        {
            builder.Configuration.Bind("AuthServer", options.ProviderOptions);
            options.UserOptions.NameClaim = AbpClaimTypes.Name;
            options.UserOptions.RoleClaim = AbpClaimTypes.Role;

            options.ProviderOptions.DefaultScopes.Add("offline_access");
            options.ProviderOptions.DefaultScopes.Remove("profile");
        });
    }

    private static void ConfigureUI(WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#ApplicationContainer");
        builder.RootComponents.Add<HeadOutlet>("head::after");
    }

    private static void ConfigureHttpClient(ServiceConfigurationContext context,
        IWebAssemblyHostEnvironment environment)
    {
        context.Services.AddTransient(sp => new HttpClient
        {
            BaseAddress = new Uri(environment.BaseAddress)
        });
    }
}