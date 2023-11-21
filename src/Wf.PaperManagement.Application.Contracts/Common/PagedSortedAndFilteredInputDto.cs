using Volo.Abp.Application.Dtos;

namespace csuwf.PaperManagement.Common;

/// <summary>
/// 分页、排序和过滤的输入DTO
/// </summary>
public class PagedSortedAndFilteredResultRequestDto:PagedAndSortedResultRequestDto
{
    public string? FilterField { get; set; }
    public string? FilterValue { get; set; }
}