// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using csuwf.PaperManagement.Workers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Modeling;

// ReSharper disable once CheckNamespace
namespace csuwf.PaperManagement.Workers;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IWorkerAppService), typeof(WorkerClientProxy))]
public partial class WorkerClientProxy : ClientProxyBase<IWorkerAppService>, IWorkerAppService
{
    public virtual async Task<WorkerDto> GetByWorkerIdAsync(int workerId)
    {
        return await RequestAsync<WorkerDto>(nameof(GetByWorkerIdAsync), new ClientProxyRequestTypeValue
        {
            { typeof(int), workerId }
        });
    }

    public virtual async Task<WorkerDto> GetByUserIdAsync(Guid userId)
    {
        return await RequestAsync<WorkerDto>(nameof(GetByUserIdAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), userId }
        });
    }

    public virtual async Task<PagedResultDto<WorkerDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        return await RequestAsync<PagedResultDto<WorkerDto>>(nameof(GetListAsync), new ClientProxyRequestTypeValue
        {
            { typeof(PagedAndSortedResultRequestDto), input }
        });
    }

    public virtual async Task<WorkerDto> CreateAsync(CreateWorkerDto input)
    {
        return await RequestAsync<WorkerDto>(nameof(CreateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(CreateWorkerDto), input }
        });
    }

    public virtual async Task<WorkerDto> UpdateAsync(Guid userId, UpdateWorkerDto input)
    {
        return await RequestAsync<WorkerDto>(nameof(UpdateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), userId },
            { typeof(UpdateWorkerDto), input }
        });
    }

    public virtual async Task<WorkerDto> DeleteByWorkerIdAsync(int workerId)
    {
        return await RequestAsync<WorkerDto>(nameof(DeleteByWorkerIdAsync), new ClientProxyRequestTypeValue
        {
            { typeof(int), workerId }
        });
    }

    public virtual async Task<WorkerDto> DeleteByUserIdAsync(Guid userId)
    {
        return await RequestAsync<WorkerDto>(nameof(DeleteByUserIdAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), userId }
        });
    }

    public virtual async Task OnDutyAsync(Guid userId)
    {
        await RequestAsync(nameof(OnDutyAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), userId }
        });
    }

    public virtual async Task OffDutyAsync(Guid userId)
    {
        await RequestAsync(nameof(OffDutyAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), userId }
        });
    }
}
