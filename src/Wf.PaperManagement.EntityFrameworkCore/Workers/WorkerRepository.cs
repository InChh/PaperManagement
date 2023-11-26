using System;
using System.Threading.Tasks;
using Wf.PaperManagement.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Wf.PaperManagement.Workers;

public class WorkerRepository : EfCoreRepository<PaperManagementDbContext, Worker>, IWorkerRepository
{
    public WorkerRepository(IDbContextProvider<PaperManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<Worker> GetByWorkerId(int workerId)
    {
        return await GetAsync(x => x.WorkerId == workerId);
    }

    public async Task<Worker> GetByUserId(Guid userId)
    {
        return await GetAsync(x => x.UserId == userId);
    }
}