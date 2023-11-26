using System;
using Volo.Abp.Application.Dtos;

namespace Wf.PaperManagement.Workers;

public class WorkerDto : FullAuditedEntityDto
{
    public Guid UserId { get; set; }
    public int WorkerId { get; set; }
    public string Name { get; set; } = null!;
}