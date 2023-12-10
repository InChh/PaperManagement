using System;
using System.ComponentModel.DataAnnotations;

namespace Wf.PaperManagement.Papers;

/// <summary>
/// 创建或更新服务单DTO
/// </summary>
public class CreateUpdatePaperDto
{

    /// <summary>
    /// 客户姓名
    /// </summary>
    [Required]
    [MaxLength(PaperConsts.MaxNameLength)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 客户联系电话
    /// </summary>
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// 地址
    /// </summary>
    [Required]
    [MaxLength(PaperConsts.MaxAddressLength)]
    public string Address { get; set; } = null!;

    /// <summary>
    /// 问题类型
    /// </summary>
    [Required]
    [MaxLength(PaperConsts.MaxProblemTypeLength)]
    public string ProblemType { get; set; } = null!;

    /// <summary>
    /// 问题描述
    /// </summary>
    [Required]
    [MaxLength(PaperConsts.MaxProblemDescriptionLength)]
    public string ProblemDescription { get; set; } = null!;

    /// <summary>
    /// 解决方法
    /// </summary>
    [MaxLength(PaperConsts.MaxSolutionLength)]
    public string? Solution { get; set; }

    /// <summary>
    /// 服务单状态
    /// </summary>
    public PaperStatus Status { get; set; } = PaperStatus.UnProcessed;

    /// <summary>
    /// 拿单队员工号
    /// </summary>
    [Required]
    public int ReceiverId { get; set; }

    /// <summary>
    /// 出单队员工号
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// 出单队员工号
    /// </summary>
    public int? Worker2Id { get; set; }

    /// <summary>
    /// 拿单时间
    /// </summary>
    [Required]
    public DateTime ReceiveTime { get; set; }

    /// <summary>
    /// 还单时间
    /// </summary>
    public DateTime? CompleteTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(PaperConsts.MaxNoteLength)]
    public string? Note { get; set; }
}