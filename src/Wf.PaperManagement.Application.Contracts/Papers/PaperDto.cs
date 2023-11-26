using System;
using Volo.Abp.Application.Dtos;

namespace Wf.PaperManagement.Papers;

public class PaperDto : FullAuditedEntityDto<Guid>
{

    /// <summary>
    /// 客户姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 客户联系电话
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 问题类型
    /// </summary>
    public string ProblemType { get; set; }

    /// <summary>
    /// 问题描述
    /// </summary>
    public string ProblemDescription { get; set; }

    /// <summary>
    /// 解决方法
    /// </summary>
    public string? Solution { get; set; }

    /// <summary>
    /// 服务单状态
    /// </summary>
    public PaperStatus Status { get; set; } = PaperStatus.UnProcessed;

    /// <summary>
    /// 拿单队员工号
    /// </summary>
    public int ReceiverId { get; set; }

    /// <summary>
    /// 拿单队员姓名
    /// </summary>
    public string? ReceiverName { get; set; }

    /// <summary>
    /// 出单队员工号
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// 出单队员姓名
    /// </summary>
    public string? WorkerName { get; set; }

    /// <summary>
    /// 出单队员工号
    /// </summary>
    public int? Worker2Id { get; set; }

    /// <summary>
    /// 出单队员姓名
    /// </summary>
    public string? Worker2Name { get; set; }

    /// <summary>
    /// 拿单时间
    /// </summary>
    public DateTime ReceiveTime { get; set; }

    /// <summary>
    /// 还单时间
    /// </summary>
    public DateTime? CompleteTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Note { get; set; }
}