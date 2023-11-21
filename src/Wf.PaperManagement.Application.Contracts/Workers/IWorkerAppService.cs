using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace csuwf.PaperManagement.Workers;

public interface IWorkerAppService : IApplicationService
{
    /// <summary>
    /// 通过工号获取队员信息
    /// </summary>
    /// <param name="workerId">工号</param>
    /// <returns></returns>
    Task<WorkerDto> GetByWorkerIdAsync(int workerId);

    /// <summary>
    /// 通过用户id获取队员信息
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <returns></returns>
    Task<WorkerDto> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// 分页查询队员信息
    /// </summary>
    /// <param name="input">分页查询信息</param>
    /// <returns></returns>
    Task<PagedResultDto<WorkerDto>> GetListAsync(PagedAndSortedResultRequestDto input);

    /// <summary>
    /// 添加新队员，将用户与工号绑定
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<WorkerDto> CreateAsync(CreateWorkerDto input);

    /// <summary>
    /// 更新队员信息，只能更新工号
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<WorkerDto> UpdateAsync(Guid userId, UpdateWorkerDto input);

    Task<WorkerDto> DeleteByWorkerIdAsync(int workerId);

    Task<WorkerDto> DeleteByUserIdAsync(Guid userId);

    /// <summary>
    /// 设置队员为在队队员，可以进行登单，否则只能查看
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <returns></returns>
    Task OnDuty(Guid userId);

    /// <summary>
    /// 设置队员不再值班，不可以进行登单，只能查看
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task OffDuty(Guid userId);
}