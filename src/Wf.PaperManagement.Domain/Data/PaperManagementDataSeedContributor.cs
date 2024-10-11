using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wf.PaperManagement.Papers;
using Wf.PaperManagement.Workers;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Wf.PaperManagement.Data;

public class PaperManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IRepository<Paper, Guid> _paperRepository;
    private readonly IRepository<Worker> _workerRepository;

    public PaperManagementDataSeedContributor(IRepository<Paper, Guid> paperRepository,
        IRepository<Worker> workerRepository, IGuidGenerator guidGenerator)
    {
        _paperRepository = paperRepository;
        _workerRepository = workerRepository;
        _guidGenerator = guidGenerator;
    }

    [UnitOfWork(false)]
    public async Task SeedAsync(DataSeedContext context)
    {
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
        if (env is "Development" or "Test")
        {
            await SeedWorkers();
            await SeedPapers();
        }
    }

    [UnitOfWork(false)]
    private async Task SeedWorkers()
    {
        var count = await _workerRepository.CountAsync();
        if (count != 0)
        {
            return;
        }

        var workers = new List<Worker>();
        for (var i = 1; i <= 50; i++)
        {
            var worker = new Worker(_guidGenerator.Create(), 400 + i, $"队员{i}",true);
            workers.Add(worker);
        }

        await _workerRepository.InsertManyAsync(workers, true);
    }


    [UnitOfWork(false)]
    private async Task SeedPapers()
    {
        var count = await _paperRepository.CountAsync();
        if (count != 0)
        {
            return;
        }

        var papers = new List<Paper>();
        for (var month = 3; month <= 8; month++)
        {
            for (var day = 1; day <= DateTime.DaysInMonth(DateTime.Now.Year, month); day++)
            {
                var time = new DateTime(DateTime.Now.Year, month, day);
                var paper = new Paper(_guidGenerator.Create(), $"张三.{month}.{day}", "15502211022", "新四舍", "笔记本/Win10",
                    "电脑开不了机", await _workerRepository.GetAsync(x => x.WorkerId == 414), time)
                {
                    Solution = "经过一番操作....",
                    Worker = await _workerRepository.GetAsync(x => x.WorkerId == 414),
                    Status = PaperStatus.Processed,
                };
                paper.SetCompleteTime(time + TimeSpan.FromHours(1));
                papers.Add(paper);
            }
        }

        for (var day = 1; day <= DateTime.DaysInMonth(DateTime.Now.Year, 4); day++)
        {
            var time = new DateTime(DateTime.Now.Year, 4, day);
            var paper = new Paper(_guidGenerator.Create(), $"李四.{4}.{day}", "15502211022", "铁三舍", "笔记本/Win10",
                "清灰", await _workerRepository.GetAsync(x => x.WorkerId == 414), time)
            {
                Solution = "经过一番操作....",
                Worker = await _workerRepository.GetAsync(x => x.WorkerId == 414),
                Worker2 = await _workerRepository.GetAsync(x => x.WorkerId == 450 - day),
                Status = PaperStatus.Processed,
            };
            paper.SetCompleteTime(time + TimeSpan.FromHours(1));
            papers.Add(paper);
        }

        await _paperRepository.InsertManyAsync(papers, true);
    }
}