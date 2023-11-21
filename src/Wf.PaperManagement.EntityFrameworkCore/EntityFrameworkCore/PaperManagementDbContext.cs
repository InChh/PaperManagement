using csuwf.PaperManagement.Papers;
using csuwf.PaperManagement.Workers;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;


namespace csuwf.PaperManagement.EntityFrameworkCore;

[ConnectionStringName("Default")]
public class PaperManagementDbContext :
    AbpDbContext<PaperManagementDbContext>
{
    public DbSet<Paper> Papers { get; set; } = null!;
    public DbSet<Worker> Workers { get; set; } = null!;

    public PaperManagementDbContext(DbContextOptions<PaperManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();

        builder.ApplyConfigurationsFromAssembly(typeof(PaperManagementDbContext).Assembly);
    }
}