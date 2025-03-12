using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Wf.PaperManagement.Papers;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
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

    public async Task<int> GetTotalResolveCountAsync()
    {
        var queryable = await _paperRepository.GetQueryableAsync();
        queryable = queryable.Where(p => p.Status == PaperStatus.Processed);
        var count = await AsyncExecuter.CountAsync(queryable);
        return count;
    }

    public async Task<int> GetMonthlyResolveCountAsync()
    {
        var queryable = await _paperRepository.GetQueryableAsync();
        queryable = queryable.Where(
            p => p.CreationTime.Month == DateTime.Now.Month && p.Status == PaperStatus.Processed);
        var count = await AsyncExecuter.CountAsync(queryable);
        return count;
    }

    public async Task<List<DailyResolveCountDto>> GetMonthlyResolveDetailAsync(int year, int month)
    {
        if (year < 0)
        {
            throw new BusinessException(null, "年份不能为负");
        }

        if (month is < 1 or > 12)
        {
            throw new BusinessException(null, "月份必须在1-12之间");
        }

        var queryable = await _paperRepository.GetQueryableAsync();
        var dailyResolveCountDtos = queryable
            .Where(p => p.CompleteTime != null)
            .Where(p => p.CompleteTime!.Value.Year == year && p.CompleteTime!.Value.Month == month &&
                        p.Status == PaperStatus.Processed)
            .GroupBy(p => p.CompleteTime!.Value.Date)
            .Select(g => new DailyResolveCountDto()
            {
                Date = g.Key,
                ResolveCount = g.Count()
            })
            .ToList();
        return dailyResolveCountDtos;
    }

    public async Task<PagedResultDto<WorkerResolveCountDto>> GetWorkerResolveCountAsync(GetWorkerResolveCountDto input)
    {
        var start = new TimeSpan(0, 0, 0);
        var startTime = input.StartTime.Date + start;
        var end = new TimeSpan(23, 59, 59);
        var endTime = input.EndTime.Date + end;
        var queryable = await _paperRepository.GetQueryableAsync();
        queryable = queryable
            .Where(p => p.CompleteTime != null)
            .Where(p =>
                p.Status == PaperStatus.Processed && p.CompleteTime! >= startTime && p.CompleteTime! <= endTime);


        var worker1Papers = queryable
            .Where(p => p.WorkerId.HasValue)
            .GroupBy(p => p.WorkerId)
            .Select(g => new WorkerResolveCountDto()
            {
                WorkerId = g.Key!.Value,
                WorkerName = g.First().Worker!.Name,
                ResolveCount = g.Count()
            });
        var worker2Papers = queryable
            .Where(p => p.Worker2Id.HasValue)
            .GroupBy(p => p.Worker2Id)
            .Select(g => new WorkerResolveCountDto()
            {
                WorkerId = g.Key!.Value,
                WorkerName = g.First().Worker2!.Name,
                ResolveCount = g.Count()
            });

        var resultQueryable = worker1Papers.Concat(worker2Papers)
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