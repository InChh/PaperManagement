using System.Threading.Tasks;

namespace csuwf.PaperManagement.Data;

public interface IPaperManagementDbSchemaMigrator
{
    Task MigrateAsync();
}
