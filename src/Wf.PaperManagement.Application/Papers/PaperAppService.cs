using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Wf.PaperManagement.Common;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace Wf.PaperManagement.Papers;

[Authorize]
public class PaperAppService : PaperManagementAppService, IPaperAppService
{
    private readonly IRepository<Paper, Guid> _paperRepository;
    private readonly PaperManager _paperManager;

    public PaperAppService(IRepository<Paper, Guid> paperRepository,
        PaperManager paperManager)
    {
        _paperRepository = paperRepository;
        _paperManager = paperManager;
    }

    /// <summary>
    /// 通过指定id获取服务单信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PaperDto> GetAsync(Guid id)
    {
        var paper = await _paperRepository.GetAsync(id);
        return ObjectMapper.Map<Paper, PaperDto>(paper);
    }

    /// <summary>
    /// 分页查询服务单，支持排序和过滤
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedResultDto<PaperDto>> GetListAsync(PagedSortedAndFilteredResultRequestDto input)
    {
        var queryable = await _paperRepository.WithDetailsAsync(p => p.Worker, p => p.Worker2, p => p.Receiver);

        // 过滤查询
        if (!(input.FilterField.IsNullOrEmpty() || input.FilterValue.IsNullOrEmpty()))
        {
            queryable = input.FilterField switch
            {
                nameof(Paper.Name) => queryable.Where(t => t.Name.Contains(input.FilterValue!)),
                nameof(Paper.WorkerId) => queryable.WhereIf(int.TryParse(input.FilterValue!, out var workerId),
                    t => t.WorkerId == workerId || t.Worker2Id == workerId),
                nameof(Paper.Status) => queryable.WhereIf(
                    Enum.TryParse<PaperStatus>(input.FilterValue!, out var status), t => t.Status == status),
                nameof(Paper.PhoneNumber) => queryable.Where(t => t.PhoneNumber.Contains(input.FilterValue!)),
                _ => queryable
            };
        }

        // 排序
        queryable = !string.IsNullOrEmpty(input.Sorting)
            ? queryable.OrderBy(input.Sorting)
            : queryable.OrderByDescending(t => t.CreationTime);

        // 获取总数
        var count = await AsyncExecuter.CountAsync(queryable);

        queryable = queryable.PageBy(input);

        var papers = await AsyncExecuter.ToListAsync(queryable);
        return new PagedResultDto<PaperDto>(count, ObjectMapper.Map<List<Paper>, List<PaperDto>>(papers));
    }


    /// <summary>
    /// 创建服务单
    /// </summary>
    /// <param name="input"></param>
    /// <returns>新创建的服务单DTO对象</returns>
    [Authorize(Roles = "worker")]
    public async Task<PaperDto> CreateAsync(CreateUpdatePaperDto input)
    {
        var paper = await _paperManager.CreateAsync(
            name: input.Name, phoneNumber: input.PhoneNumber, address: input.Address
            , problemType: input.ProblemType, problemDescription: input.ProblemDescription
            , receiverId: input.ReceiverId, receiveTime: input.ReceiveTime
            , status: input.Status);

        paper.Solution = input.Solution;
        paper.Note = input.Note;

        paper.SetCompleteTime(input.CompleteTime);
        await _paperManager.SetWorkerAsync(paper, input.WorkerId);
        await _paperManager.SetWorker2Async(paper, input.Worker2Id);
        await _paperRepository.InsertAsync(paper);
        return ObjectMapper.Map<Paper, PaperDto>(paper);
    }

    /// <summary>
    /// 更新服务单
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize(Roles = "worker")]
    public async Task<PaperDto> UpdateAsync(Guid id, CreateUpdatePaperDto input)
    {
        var paper = await _paperRepository.GetAsync(id);

        paper.SetName(input.Name);
        paper.SetAddress(input.Address);
        paper.SetPhoneNumber(input.PhoneNumber);
        paper.SetProblemType(input.ProblemType);
        paper.SetProblemDescription(input.ProblemDescription);
        paper.SetCompleteTime(input.CompleteTime);
        paper.Solution = input.Solution;
        paper.Note = input.Note;
        await _paperManager.SetReceiverAsync(paper, input.ReceiverId);
        await _paperManager.SetWorkerAsync(paper, input.WorkerId);
        await _paperManager.SetWorker2Async(paper, input.Worker2Id);

        await _paperRepository.UpdateAsync(paper);
        return ObjectMapper.Map<Paper, PaperDto>(paper);
    }

    /// <summary>
    /// 删除服务单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Roles = "admin")]
    public async Task<PaperDto> DeleteAsync(Guid id)
    {
        var paper = await _paperRepository.GetAsync(id);
        await _paperRepository.DeleteAsync(id);
        return ObjectMapper.Map<Paper, PaperDto>(paper);
    }
}