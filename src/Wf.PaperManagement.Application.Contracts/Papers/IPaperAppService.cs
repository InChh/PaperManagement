using System;
using System.Threading.Tasks;
using Wf.PaperManagement.Common;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Wf.PaperManagement.Papers;

public interface IPaperAppService : IApplicationService
{
    /// <summary>
    /// 通过指定id获取服务单信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<PaperDto> GetAsync(Guid id);

    /// <summary>
    /// 分页查询服务单，支持排序和过滤
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<PaperDto>> GetListAsync(PagedSortedAndFilteredResultRequestDto input);
    
    /// <summary>
    /// 创建服务单
    /// </summary>
    /// <param name="input"></param>
    /// <returns>新创建的服务单DTO对象</returns>
    Task<PaperDto> CreateAsync(CreateUpdatePaperDto input);

    /// <summary>
    /// 更新服务单
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PaperDto> UpdateAsync(Guid id, CreateUpdatePaperDto input);

    /// <summary>
    /// 删除服务单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<PaperDto> DeleteAsync(Guid id);
}