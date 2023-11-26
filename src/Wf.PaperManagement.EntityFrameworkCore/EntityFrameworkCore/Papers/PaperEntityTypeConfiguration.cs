using Wf.PaperManagement.Papers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Wf.PaperManagement.EntityFrameworkCore.Papers;

/// <summary>
/// 服务单实体配置
/// </summary>
public class PaperEntityTypeConfiguration : IEntityTypeConfiguration<Paper>
{
    public void Configure(EntityTypeBuilder<Paper> builder)
    {
        builder.ToTable(PaperManagementConsts.DbTablePrefix + "Papers",PaperManagementConsts.DbSchema);
        builder.ConfigureByConvention();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(PaperConsts.MaxNameLength);
        builder.Property(x => x.PhoneNumber).IsRequired();
        builder.Property(x => x.Address).IsRequired().HasMaxLength(PaperConsts.MaxAddressLength);
        builder.Property(x => x.ProblemType).IsRequired().HasMaxLength(PaperConsts.MaxProblemTypeLength);
        builder.Property(x => x.ProblemDescription).IsRequired().HasMaxLength(PaperConsts.MaxProblemDescriptionLength);
        builder.Property(x => x.Solution).HasMaxLength(PaperConsts.MaxSolutionLength);
        builder.Property(x => x.Status).IsRequired().HasDefaultValue(PaperStatus.UnProcessed);
        builder.Property(x => x.ReceiveTime).IsRequired();
        builder.Property(x => x.ReceiverId).IsRequired();
        builder.HasOne(x => x.Receiver).WithMany().HasForeignKey(x => x.ReceiverId);
        builder.Property(x => x.Note).HasMaxLength(PaperConsts.MaxNoteLength);
        builder.HasOne(x => x.Worker).WithMany().HasForeignKey(x => x.WorkerId);
        builder.HasOne(x => x.Worker2).WithMany().HasForeignKey(x => x.Worker2Id);
    }
}