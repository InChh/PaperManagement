using System.Threading.Tasks;

namespace Wf.PaperManagement.Data;

public interface IPaperManagementDbSchemaMigrator
{
    Task MigrateAsync();
}
