// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using csuwf.PaperManagement.Papers;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

// ReSharper disable once CheckNamespace
namespace csuwf.PaperManagement.Papers;

public class PaperDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public string ProblemType { get; set; }

    public string ProblemDescription { get; set; }

    public string Solution { get; set; }

    public PaperStatus Status { get; set; }

    public int ReceiverId { get; set; }

    public string ReceiverName { get; set; }

    public int? WorkerId { get; set; }

    public string WorkerName { get; set; }

    public int? Worker2Id { get; set; }

    public string Worker2Name { get; set; }

    public DateTime ReceiveTime { get; set; }

    public DateTime? CompleteTime { get; set; }

    public string Note { get; set; }
}
