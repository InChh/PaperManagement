using System;
using System.Text.RegularExpressions;
using csuwf.PaperManagement.Workers;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Validation;

namespace csuwf.PaperManagement.Papers;

public class Paper : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 客户姓名
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 客户联系电话
    /// </summary>
    public string PhoneNumber { get; private set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; private set; }

    /// <summary>
    /// 问题类型
    /// </summary>
    public string ProblemType { get; private set; }

    /// <summary>
    /// 问题描述
    /// </summary>
    public string ProblemDescription { get; private set; }

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

    public Worker Receiver { get; set; }

    /// <summary>
    /// 出单队员工号
    /// </summary>
    public int? WorkerId { get; set; }

    public Worker? Worker { get; set; }

    /// <summary>
    /// 出单队员工号
    /// </summary>
    public int? Worker2Id { get; set; }

    public Worker? Worker2 { get; set; }

    /// <summary>
    /// 拿单时间
    /// </summary>
    public DateTime ReceiveTime { get; set; }

    /// <summary>
    /// 还单时间
    /// </summary>
    public DateTime? CompleteTime { get; private set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Note { get; set; }

    protected Paper()
    {
    }

    public Paper(Guid id, string name, string phoneNumber, string address, string problemType,
        string problemDescription, Worker receiver, DateTime receiveTime) : base(id)
    {
        SetName(name);
        SetPhoneNumber(phoneNumber);
        SetAddress(address);
        SetProblemType(problemType);
        SetProblemDescription(problemDescription);
        Receiver = receiver;
        ReceiveTime = receiveTime;
    }

    public void SetName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: PaperConsts.MaxNameLength);
        Name = name;
    }

    public void SetPhoneNumber(string phone)
    {
        Check.NotNullOrWhiteSpace(phone, nameof(phone));
        PhoneNumber = phone;
    }

    public void SetAddress(string address)
    {
        Check.NotNullOrWhiteSpace(address, nameof(address), maxLength: PaperConsts.MaxAddressLength);
        Address = address;
    }

    public void SetProblemType(string problemType)
    {
        Check.NotNullOrWhiteSpace(problemType, nameof(problemType), maxLength: PaperConsts.MaxProblemTypeLength);
        ProblemType = problemType;
    }

    public void SetProblemDescription(string problemDescription)
    {
        Check.NotNullOrWhiteSpace(problemDescription, nameof(problemDescription),
            maxLength: PaperConsts.MaxProblemDescriptionLength);
        ProblemDescription = problemDescription;
    }

    public void SetCompleteTime(DateTime? completeTime)
    {
        if (completeTime is null)
        {
            return;
        }

        if (completeTime < ReceiveTime)
        {
            throw new BusinessException(PaperManagementDomainErrorCodes.CompleteTimeShouldLaterThanReceiveTime);
        }

        CompleteTime = completeTime;
    }
}