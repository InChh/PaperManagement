using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wf.PaperManagement.Common;

namespace Wf.PaperManagement.Statistics;

public class GetWorkerResolveCountDto : PagedSortedAndFilteredResultRequestDto, IValidatableObject
{
    [Required] public DateTime StartTime { get; set; }
    [Required] public DateTime EndTime { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartTime > EndTime)
        {
            yield return new ValidationResult("开始时间不能晚于结束时间", new[] { nameof(StartTime), nameof(EndTime) });
        }
    }
}