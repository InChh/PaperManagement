using System;
using System.IO;
using System.Linq;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using csuwf.PaperManagement.EntityFrameworkCore;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace csuwf.PaperManagement;

[DependsOn(
    typeof(PaperManagementHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpDistributedLockingModule),
    typeof(PaperManagementApplicationModule),
    typeof(PaperManagementEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
// ReSharper disable once ClassNeverInstantiated.Global
public class PaperManagementHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureConventionalControllers();
        ConfigureAuthentication(context, configuration);
        ConfigureAuthorization(context, configuration);
        ConfigureKeycloak(context, configuration);
        ConfigureCache(configuration);
        ConfigureVirtualFileSystem(context);
        ConfigureDataProtection(context, configuration, hostingEnvironment);
        ConfigureDistributedLocking(context, configuration);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
        ConfigureAntiForgery(hostingEnvironment);

        context.Services.AddControllers(options => { options.Filters.Add<RefitExceptionHandlerFilter>(); });
    }

    private void ConfigureAntiForgery(IHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpAntiForgeryOptions>(options =>
            {
                options.AutoValidateIgnoredHttpMethods.Add("POST");
                options.AutoValidateIgnoredHttpMethods.Add("DELETE");
            });
        }
    }

    private void ConfigureKeycloak(ServiceConfigurationContext context, IConfiguration configuration)
    {
        Configure<KeycloakAdminClientOptions>(configuration.GetSection("Keycloak:Admin"));
        context.Services.AddKeycloakAdminHttpClient(configuration, null, "Keycloak:Admin");
    }

    private void ConfigureCache(IConfiguration configuration)
    {
        Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "PaperManagement:"; });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<PaperManagementDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Wf.PaperManagement.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<PaperManagementDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Wf.PaperManagement.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<PaperManagementApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Wf.PaperManagement.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<PaperManagementApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Wf.PaperManagement.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(PaperManagementApplicationModule).Assembly);
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        // context.Services.AddKeycloakAuthentication(configuration);
        var keycloakOptions = new KeycloakAuthenticationOptions();
        configuration.GetSection("Keycloak").Bind(keycloakOptions, opt => opt.BindNonPublicProperties = true);

        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                var validationParameters = new TokenValidationParameters()
                {
                    ClockSkew = keycloakOptions.TokenClockSkew,
                    ValidateAudience = keycloakOptions.VerifyTokenAudience ?? true,
                    ValidateIssuer = true,
                };

                var sslRequired = string.IsNullOrWhiteSpace(keycloakOptions.SslRequired)
                                  || keycloakOptions.SslRequired
                                      .Equals("external", StringComparison.OrdinalIgnoreCase);

                opts.Authority = keycloakOptions.KeycloakUrlRealm;
                opts.Audience = keycloakOptions.Resource;
                opts.TokenValidationParameters = validationParameters;
                opts.RequireHttpsMetadata = sslRequired;
                opts.SaveToken = true;
            });
    }

    private void ConfigureAuthorization(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthorization(options => { });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOidc(authority: configuration["Keycloak:auth-server-url"]!,
            scopes: new string[] { },
            flows: new[] { AbpSwaggerOidcFlows.AuthorizationCode, AbpSwaggerOidcFlows.Password },
            discoveryEndpoint: configuration["Keycloak:auth-server-url"]!.TrimEnd('/') + "/realms/" +
                               configuration["Keycloak:realm"]!, options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PaperManagement API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                //Hides ABP Related endpoints on Swagger UI
                options.HideAbpEndpoints();
            });
    }

    private void ConfigureDataProtection(
        ServiceConfigurationContext context,
        IConfiguration configuration,
        IWebHostEnvironment hostingEnvironment)
    {
        var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("PaperManagement");
        if (!hostingEnvironment.IsDevelopment())
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "PaperManagement-Protection-Keys");
        }
    }

    private void ConfigureDistributedLocking(
        ServiceConfigurationContext context,
        IConfiguration configuration)
    {
        context.Services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();

        app.UseAuthorization();


        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PaperManagement API");

                var configuration = context.GetConfiguration();
                options.OAuthClientId(configuration["Keycloak:resource"]);
                options.OAuthClientSecret(configuration["Keycloak:Credentials:secret"]);
            });
        }

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
    }
}