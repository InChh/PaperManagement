using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Wf.PaperManagement.Papers;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Wf.PaperManagement.Statistics;

[Authorize]
public class StatisticAppService : PaperManagementAppService, IStatisticAppService
{
    private readonly IRepository<Paper, Guid> _paperRepository;

    public StatisticAppService(IRepository<Paper, Guid> paperRepository)
    {
        _paperRepository = paperRepository;
    }

    public async Task<int> GetTodayResolveCountAsync()
    {
        var queryable = await _paperRepository.GetQueryableAsync();
        queryable = queryable.Where(p => p.CreationTime.Date == DateTime.Now.Date && p.Status == PaperStatus.Processed);
        var count = await AsyncExecuter.CountAsync(queryable);
        return count;
    }

    public async Task<PagedResultDto<WorkerResolveCountDto>> GetWorkerResolveCountAsync(GetWorkerResolveCountDto input)
    {
        var queryable = await _paperRepository.GetQueryableAsync();
        queryable = queryable.Where(p =>
            p.Status == PaperStatus.Processed && p.CompleteTime >= input.StartTime && p.CompleteTime <= input.EndTime);

        var worker1Papers = queryable.GroupBy(p => p.WorkerId)
            .Where(p => p.Key != null)
            .Select(g => new WorkerResolveCountDto()
            {
                WorkerId = g.Key!.Value,
                WorkerName = g.First().Name,
                ResolveCount = g.Count()
            });
        var worker2Papers = queryable.GroupBy(p => p.Worker2Id)
            .Where(p => p.Key != null)
            .Select(g => new WorkerResolveCountDto()
            {
                WorkerId = g.Key!.Value,
                WorkerName = g.First().Name,
                ResolveCount = g.Count()
            });

        var resultQueryable = worker1Papers.Union(worker2Papers)
            .GroupBy(p => p.WorkerId)
            .Select(g => new WorkerResolveCountDto()
            {
                WorkerId = g.Key,
                WorkerName = g.First().WorkerName,
                ResolveCount = g.Sum(p => p.ResolveCount)
            });

        if (!(input.FilterValue.IsNullOrWhiteSpace() || input.FilterValue.IsNullOrWhiteSpace()))
        {
            resultQueryable = input.FilterField switch
            {
                nameof(WorkerResolveCountDto.WorkerId) => resultQueryable.WhereIf(
                    int.TryParse(input.FilterValue, out var workerId),
                    p => p.WorkerId == workerId),
                nameof(WorkerResolveCountDto.WorkerName) => resultQueryable.Where(p =>
                    p.WorkerName.Contains(input.FilterValue!)),
                _ => resultQueryable
            };
        }

        resultQueryable = !input.Sorting.IsNullOrWhiteSpace()
            ? resultQueryable.OrderBy(input.Sorting!)
            : resultQueryable.OrderByDescending(p => p.ResolveCount);

        var totalCount = await AsyncExecuter.CountAsync(resultQueryable);

        resultQueryable = resultQueryable.PageBy(input);
        var result = await AsyncExecuter.ToListAsync(resultQueryable);

        return new PagedResultDto<WorkerResolveCountDto>(totalCount, result);
    }
}