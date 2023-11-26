using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wf.PaperManagement.Data;
using Volo.Abp.DependencyInjection;

namespace Wf.PaperManagement.EntityFrameworkCore;

public class EntityFrameworkCorePaperManagementDbSchemaMigrator
    : IPaperManagementDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCorePaperManagementDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the PaperManagementDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<PaperManagementDbContext>()
            .Database
            .MigrateAsync();
    }
}
