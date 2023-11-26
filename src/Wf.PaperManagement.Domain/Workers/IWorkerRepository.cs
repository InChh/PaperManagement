using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Wf.PaperManagement.Workers;

public interface IWorkerRepository : IRepository<Worker>
{
    Task<Worker> GetByWorkerId(int workerId);

    Task<Worker> GetByUserId(Guid userId);
}