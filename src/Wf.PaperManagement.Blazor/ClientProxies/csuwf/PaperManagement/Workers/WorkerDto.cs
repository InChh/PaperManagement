// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using csuwf.PaperManagement.Workers;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

// ReSharper disable once CheckNamespace
namespace csuwf.PaperManagement.Workers;

public class WorkerDto : FullAuditedEntityDto
{
    public Guid UserId { get; set; }

    public int WorkerId { get; set; }

    public string Name { get; set; }
}
