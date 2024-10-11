using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Keycloak.AuthServices.Sdk;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Wf.PaperManagement.Workers;

public class WorkerManager : DomainService
{
    private readonly IKeycloakUserClient _keycloakUserClient;
    private readonly IKeycloakClient _keycloakClient;
    private readonly IOptions<KeycloakAdminClientOptions> _keycloakOptions;
    private readonly IWorkerRepository _workerRepository;

    public WorkerManager(IKeycloakUserClient keycloakUserClient,
        IOptions<KeycloakAdminClientOptions> keycloakOptions, IWorkerRepository workerRepository,
        IKeycloakClient keycloakClient)
    {
        _keycloakUserClient = keycloakUserClient;
        _keycloakOptions = keycloakOptions;
        _workerRepository = workerRepository;
        _keycloakClient = keycloakClient;
    }

    public async Task<Worker> CreateAsync(Guid userId, int workerId, string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name), WorkerConsts.MaxNameLength);

        // 检查用户是否存在
        _ = await _keycloakUserClient.GetUserAsync(_keycloakOptions.Value.Realm, userId.ToString());

        var worker = new Worker(userId, workerId, name,true);
        await SetWorkerId(worker, workerId);
        await OnDuty(worker);
        return worker;
    }

    public async Task SetWorkerId(Worker worker, int workerId)
    {
        var c = await _workerRepository.CountAsync(x => x.WorkerId == workerId);
        if (c != 0)
        {
            throw new BusinessException(PaperManagementDomainErrorCodes.WorkerIdAlreadyExists);
        }

        worker.WorkerId = workerId;
    }

    public async Task OnDuty(Worker worker)
    {
        var userId = worker.UserId;
        var groupId = (await _keycloakClient.GetGroupsAsync(_keycloakOptions.Value.Realm))
            .Where(r => r.Name!.Contains(WorkerConsts.WorkerGroupName))
            .Select(r => r.Id).Single();

        await _keycloakClient.JoinGroupAsync(_keycloakOptions.Value.Realm, userId.ToString(), groupId!);
        worker.IsOnDuty = true;
    }

    public async Task OffDuty(Worker worker)
    {
        var userId = worker.UserId;
        var groupId = (await _keycloakClient.GetGroupsAsync(_keycloakOptions.Value.Realm))
            .Where(r => r.Name!.Contains(WorkerConsts.WorkerGroupName))
            .Select(r => r.Id).Single();

        await _keycloakClient.LeaveGroupAsync(_keycloakOptions.Value.Realm, userId.ToString(), groupId!);
        worker.IsOnDuty = false;
    }
}