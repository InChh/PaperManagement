using System;
using System.Threading.Tasks;
using csuwf.PaperManagement.Workers;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace csuwf.PaperManagement.Papers;

public class PaperManager : DomainService
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IGuidGenerator _guidGenerator;

    public PaperManager(IWorkerRepository workerRepository,
        IGuidGenerator guidGenerator)
    {
        _workerRepository = workerRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task<Paper> CreateAsync(string name, string phoneNumber, string address, string problemType,
        string problemDescription, int receiverId, DateTime receiveTime, PaperStatus status = PaperStatus.UnProcessed,
        string? solution = null, int? workerId = null, int? worker2Id = null, DateTime? completeTime = null,
        string? note = null)
    {
        var receiver = await _workerRepository.GetByWorkerId(receiverId);

        var paper = new Paper(_guidGenerator.Create(), name, phoneNumber, address, problemType, problemDescription,
            receiver, receiveTime)
        {
            Solution = solution,
            Status = status,
            Note = note,
        };

        await SetWorkerAsync(paper, workerId);
        await SetWorker2Async(paper, worker2Id);
        paper.SetCompleteTime(completeTime);
        return paper;
    }

    public async Task SetReceiverAsync(Paper paper, int receiverId)
    {
        paper.Receiver = await _workerRepository.GetByWorkerId(receiverId);
    }

    public async Task SetWorkerAsync(Paper paper, int? workerId)
    {
        if (workerId is not null)
        {
            paper.Worker = await _workerRepository.GetByWorkerId(workerId.Value);
        }
    }

    public async Task SetWorker2Async(Paper paper, int? worker2Id)
    {
        if (worker2Id is not null)
        {
            paper.Worker2 = await _workerRepository.GetByWorkerId(worker2Id.Value);
        }
    }
}