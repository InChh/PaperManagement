using csuwf.PaperManagement.Papers;
using csuwf.PaperManagement.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace csuwf.PaperManagement.EntityFrameworkCore.Papers;

/// <summary>
/// 队员实体配置
/// </summary>
public class WorkerEntityTypeConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.ToTable(PaperManagementConsts.DbTablePrefix + "Workers",PaperManagementConsts.DbSchema);
        builder.ConfigureByConvention();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(PaperConsts.MaxNameLength);
        builder.Property(x => x.WorkerId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
    }
}