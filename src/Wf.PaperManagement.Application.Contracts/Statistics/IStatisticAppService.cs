using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wf.PaperManagement.Common;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Wf.PaperManagement.Statistics;

public interface IStatisticAppService : IApplicationService
{
    /// <summary>
    /// 获取今日已处理的服务单数量
    /// </summary>
    /// <returns>处理单数</returns>
    Task<int> GetTodayResolveCountAsync();

    /// <summary>
    /// 获取全部已处理的服务单数量
    /// </summary>
    /// <returns>处理单数</returns>
    Task<int> GetTotalResolveCountAsync();

    /// <summary>
    /// 获取本月已处理的服务单数量
    /// </summary>
    /// <returns>处理单数</returns>
    Task<int> GetMonthlyResolveCountAsync();

    Task<List<DailyResolveCountDto>> GetMonthlyResolveDetailAsync(int year, int month);

    /// <summary>
    /// 获取队员在指定时间范围内的出单数量
    /// </summary>
    /// <param name="input">指定开始时间和结束时间</param>
    /// <returns></returns>
    Task<PagedResultDto<WorkerResolveCountDto>> GetWorkerResolveCountAsync(GetWorkerResolveCountDto input);
}