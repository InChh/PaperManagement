using System;
using System.ComponentModel.DataAnnotations;

namespace Wf.PaperManagement.Workers;

public class CreateWorkerDto
{
    [Required] public Guid UserId { get; set; }
    [Required] public int WorkerId { get; set; }
    [Required] public string Name { get; set; } = null!;

}