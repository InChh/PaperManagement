using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac.WebAssembly;
using Volo.Abp.Http.Client.IdentityModel.WebAssembly;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Wf.PaperManagement.Blazor.Shared;

namespace Wf.PaperManagement.Blazor;

[DependsOn(
    typeof(AbpAutofacWebAssemblyModule),
    typeof(PaperManagementHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelWebAssemblyModule)
)]
public class PaperManagementBlazorModule : AbpModule
{
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var environment = context.Services.GetSingletonInstance<IWebAssemblyHostEnvironment>();
        var builder = context.Services.GetSingletonInstance<WebAssemblyHostBuilder>();
        AbpClaimTypes.UserName = ClaimTypes.Name;
        AbpClaimTypes.Name = ClaimTypes.GivenName;
        AbpClaimTypes.SurName = ClaimTypes.Surname;
        AbpClaimTypes.Role = ClaimTypes.Role;
        AbpClaimTypes.Email = ClaimTypes.Email;

        ConfigureAuthentication(builder);
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
        }).AddI18nForWasmAsync($"{builder.HostEnvironment.BaseAddress}/i18n");

        await builder.Services.AddGlobalForWasmAsync(builder.HostEnvironment.BaseAddress);
    }

    private static void ConfigureAuthentication(WebAssemblyHostBuilder builder)
    {
        builder.Services
            .AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("AuthServer", options.ProviderOptions);
                options.UserOptions.NameClaim = AbpClaimTypes.Name;
                options.UserOptions.RoleClaim = AbpClaimTypes.Role;
                //
                options.ProviderOptions.DefaultScopes.Add("offline_access");
                options.ProviderOptions.DefaultScopes.Remove("profile");
            }).AddAccountClaimsPrincipalFactory<CustomUserFactory>();
    }

    private static void ConfigureUI(WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#ApplicationContainer");
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