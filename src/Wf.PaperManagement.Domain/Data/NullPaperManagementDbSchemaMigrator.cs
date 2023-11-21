using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace csuwf.PaperManagement.Data;

/* This is used if database provider does't define
 * IPaperManagementDbSchemaMigrator implementation.
 */
public class NullPaperManagementDbSchemaMigrator : IPaperManagementDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
