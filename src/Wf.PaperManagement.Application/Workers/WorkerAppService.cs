using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Wf.PaperManagement.Workers;

[Authorize]
public class WorkerAppService : PaperManagementAppService, IWorkerAppService
{
    private readonly WorkerManager _workerManager;
    private readonly IWorkerRepository _workerRepository;
    private readonly IDataFilter<ISoftDelete> _softDeleteFilter;


    public WorkerAppService(WorkerManager workerManager, IWorkerRepository workerRepository,
        IDataFilter<ISoftDelete> softDeleteFilter)
    {
        _workerManager = workerManager;
        _workerRepository = workerRepository;
        _softDeleteFilter = softDeleteFilter;
    }

    public async Task<WorkerDto> GetByWorkerIdAsync(int workerId)
    {
        var worker = await _workerRepository.GetByWorkerId(workerId);
        return ObjectMapper.Map<Worker, WorkerDto>(worker);
    }

    public async Task<WorkerDto> GetByUserIdAsync(Guid userId)
    {
        var worker = await _workerRepository.GetAsync(t => t.UserId == userId);
        return ObjectMapper.Map<Worker, WorkerDto>(worker);
    }

    public async Task<PagedResultDto<WorkerDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var queryable = await _workerRepository.GetQueryableAsync();

        // 排序
        queryable = !input.Sorting.IsNullOrEmpty()
            ? queryable.OrderBy(input.Sorting!)
            : queryable.OrderByDescending(t => t.CreationTime);

        // 获取总数
        var count = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable.PageBy(input);

        var workers = await AsyncExecuter.ToListAsync(queryable);
        return new PagedResultDto<WorkerDto>(count, ObjectMapper.Map<List<Worker>, List<WorkerDto>>(workers));
    }

    [Authorize(Roles = "leader")]
    public async Task<WorkerDto> CreateAsync(CreateWorkerDto input)
    {
        var worker = await _workerManager.CreateAsync(input.UserId, input.WorkerId, input.Name);
        await _workerRepository.InsertAsync(worker);
        return ObjectMapper.Map<Worker, WorkerDto>(worker);
    }

    [Authorize(Roles = "leader")]
    public async Task<WorkerDto> UpdateAsync(Guid userId, UpdateWorkerDto input)
    {
        var worker = await _workerRepository.GetByUserId(userId);

        if (input.WorkerId is not null)
        {
            await _workerManager.SetWorkerId(worker, input.WorkerId.Value);
        }

        if (input.Name is not null && input.Name != worker.Name)
        {
            worker.SetName(input.Name);
        }

        await _workerRepository.UpdateAsync(worker);
        return ObjectMapper.Map<Worker, WorkerDto>(worker);
    }

    [Authorize(Roles = "leader")]
    public async Task<WorkerDto> DeleteByWorkerIdAsync(int workerId)
    {
        using (_softDeleteFilter.Disable())
        {
            var worker = await _workerRepository.GetByWorkerId(workerId);
            await _workerManager.OffDuty(worker);
            await _workerRepository.DeleteAsync(worker);
            return ObjectMapper.Map<Worker, WorkerDto>(worker);
        }
    }

    [Authorize(Roles = "leader")]
    public async Task<WorkerDto> DeleteByUserIdAsync(Guid userId)
    {
        using (_softDeleteFilter.Disable())
        {
            var worker = await _workerRepository.GetByUserId(userId);
            await _workerManager.OffDuty(worker);
            await _workerRepository.DeleteAsync(worker);
            return ObjectMapper.Map<Worker, WorkerDto>(worker);
        }
    }

    [Authorize(Roles = "leader")]
    public async Task OnDutyAsync(Guid userId)
    {
        var worker = await _workerRepository.GetByUserId(userId);
        await _workerManager.OnDuty(worker);
        await _workerRepository.UpdateAsync(worker);
    }

    [Authorize(Roles = "leader")]
    public async Task OffDutyAsync(Guid userId)
    {
        var worker = await _workerRepository.GetByUserId(userId);
        await _workerManager.OffDuty(worker);
        await _workerRepository.UpdateAsync(worker);
    }
}