using System;
using System.Threading.Tasks;
using Wf.PaperManagement.Common;
using Shouldly;
using Xunit;

namespace Wf.PaperManagement.Papers;

public sealed class PaperAppServiceTest : PaperManagementApplicationTestBase
{
    private readonly PaperAppService _paperAppService;

    public PaperAppServiceTest()
    {
        _paperAppService = GetRequiredService<PaperAppService>();
    }

    [Fact]
    public async Task ShouldPaged()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _paperAppService.GetListAsync(new PagedSortedAndFilteredResultRequestDto()
            {
                SkipCount = 0,
                MaxResultCount = 20,
            });
            result.TotalCount.ShouldBe(214);
            result.Items.Count.ShouldBe(20);
        });
    }

    [Fact]
    public async Task ShouldPagedFiltered()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var result = await _paperAppService.GetListAsync(new PagedSortedAndFilteredResultRequestDto()
            {
                FilterField = "Name",
                FilterValue = "张三",
            });
            result.TotalCount.ShouldBe(184);
            result.Items.Count.ShouldBe(10);

            result = await _paperAppService.GetListAsync(new PagedSortedAndFilteredResultRequestDto()
            {
                FilterField = "WorkerId",
                FilterValue = "414",
            });
            result.TotalCount.ShouldBe(214);
            result.Items.Count.ShouldBe(10);

            result = await _paperAppService.GetListAsync(new PagedSortedAndFilteredResultRequestDto()
            {
                FilterField = "WorkerId",
                FilterValue = "420",
            });
            result.TotalCount.ShouldBe(1);
            result.Items.Count.ShouldBe(1);
            result.Items[0].Name.ShouldBe("李四.4.30");
            result = await _paperAppService.GetListAsync(new PagedSortedAndFilteredResultRequestDto()
            {
                FilterField = "Status",
                FilterValue = "0",
            });
            result.TotalCount.ShouldBe(0);
            result.Items.Count.ShouldBe(0);
            result = await _paperAppService.GetListAsync(new PagedSortedAndFilteredResultRequestDto()
            {
                FilterField = "123",
                FilterValue = "0",
            });
            result.TotalCount.ShouldBe(214);
            result.Items.Count.ShouldBe(10);
            result.Items[0].Name.ShouldBe("李四.4.30");
        });
    }
}