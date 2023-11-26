using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Microsoft.Extensions.Options;
using Refit;
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
        _ = await _keycloakUserClient.GetUser(_keycloakOptions.Value.Realm, userId.ToString());

        var worker = new Worker(userId, workerId, name);
        await SetWorkerId(worker, workerId);
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

    public async Task OnDuty(Guid userId)
    {
        await ModifyUserGroups(userId, groups =>
        {
            if (!groups.Contains(WorkerConsts.WorkerGroupName))
            {
                groups.Add(WorkerConsts.WorkerGroupName);
            }
        });
    }

    public async Task OffDuty(Guid userId)
    {
        await ModifyUserGroups(userId, groups =>
        {
            if (groups.Contains(WorkerConsts.WorkerGroupName))
            {
                groups.Remove(WorkerConsts.WorkerGroupName);
            }
        });
    }

    private async Task ModifyUserGroups(Guid userId, Action<List<string>> action)
    {
        var groups = (await _keycloakUserClient.GetUserGroups(_keycloakOptions.Value.Realm, userId.ToString()))
            .Select(g => g.Name!).ToList();
        var u = await _keycloakUserClient.GetUser(_keycloakOptions.Value.Realm, userId.ToString());

        action(groups);

        var user = new User()
        {
            Id = u.Id,
            Groups = groups.ToArray()
        };
        await _keycloakUserClient.UpdateUser(_keycloakOptions.Value.Realm, userId.ToString(), user);
    }
}