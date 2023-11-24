// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using csuwf.PaperManagement.Workers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

// ReSharper disable once CheckNamespace
namespace csuwf.PaperManagement.Workers;

public interface IWorkerAppService : IApplicationService
{
    Task<WorkerDto> GetByWorkerIdAsync(int workerId);

    Task<WorkerDto> GetByUserIdAsync(Guid userId);

    Task<PagedResultDto<WorkerDto>> GetListAsync(PagedAndSortedResultRequestDto input);

    Task<WorkerDto> CreateAsync(CreateWorkerDto input);

    Task<WorkerDto> UpdateAsync(Guid userId, UpdateWorkerDto input);

    Task<WorkerDto> DeleteByWorkerIdAsync(int workerId);

    Task<WorkerDto> DeleteByUserIdAsync(Guid userId);

    Task OnDutyAsync(Guid userId);

    Task OffDutyAsync(Guid userId);
}