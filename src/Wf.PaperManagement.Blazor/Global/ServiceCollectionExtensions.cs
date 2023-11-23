using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using Wf.PaperManagement.Blazor.Global.Config;
using Wf.PaperManagement.Blazor.Global;
using Wf.PaperManagement.Blazor.Global.Nav.Model;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGlobalForServer(this IServiceCollection services)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new Exception("Get the assembly root directory exception!");
            services.AddNav(Path.Combine(basePath, $"wwwroot/nav/nav.json"));
            services.AddScoped<CookieStorage>();
            services.AddScoped<GlobalConfig>();

            return services;
        }

        public static async Task<IServiceCollection> AddGlobalForWasmAsync(this IServiceCollection services, string baseUri)
        {
            using var httpclient = new HttpClient();
            var navList = await httpclient.GetFromJsonAsync<List<NavModel>>(Path.Combine(baseUri, $"nav/nav.json")) ?? throw new Exception("please configure the Navigation!");
            services.AddNav(navList);
            services.AddScoped<CookieStorage>();
            services.AddScoped<GlobalConfig>();

            return services;
        }
    }
}
