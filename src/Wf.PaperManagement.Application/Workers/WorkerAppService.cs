using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Wf.PaperManagement.Workers;

[Authorize]
public class WorkerAppService : PaperManagementAppService, IWorkerAppService
{
    private readonly WorkerManager _workerManager;
    private readonly IWorkerRepository _workerRepository;

    public WorkerAppService(WorkerManager workerManager, IWorkerRepository workerRepository)
    {
        _workerManager = workerManager;
        _workerRepository = workerRepository;
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

    [Authorize(Roles = "admin")]
    public async Task<WorkerDto> CreateAsync(CreateWorkerDto input)
    {
        var worker = await _workerManager.CreateAsync(input.UserId, input.WorkerId,input.Name);
        await _workerRepository.InsertAsync(worker);
        return ObjectMapper.Map<Worker, WorkerDto>(worker);
    }

    [Authorize(Roles = "admin")]
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

    [Authorize(Roles = "admin")]
    public async Task<WorkerDto> DeleteByWorkerIdAsync(int workerId)
    {
        var worker = await _workerRepository.GetByWorkerId(workerId);
        await _workerRepository.DeleteAsync(worker);
        return ObjectMapper.Map<Worker, WorkerDto>(worker);
    }

    [Authorize(Roles = "admin")]
    public async Task<WorkerDto> DeleteByUserIdAsync(Guid userId)
    {
        var worker = await _workerRepository.GetByUserId(userId);
        await _workerRepository.DeleteAsync(worker);
        return ObjectMapper.Map<Worker, WorkerDto>(worker);
    }

    public async Task OnDutyAsync(Guid userId)
    {
        await _workerManager.OnDuty(userId);
    }

    public async Task OffDutyAsync(Guid userId)
    {
        await _workerManager.OffDuty(userId);
    }
}