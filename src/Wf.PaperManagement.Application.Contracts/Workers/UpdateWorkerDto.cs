using System.ComponentModel.DataAnnotations;

namespace csuwf.PaperManagement.Workers;

public class UpdateWorkerDto
{
    /// <summary>
    /// 队员工号
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// 队员姓名
    /// </summary>
    [MaxLength(WorkerConsts.MaxNameLength)]
    public string? Name { get; set; } = null!;
}