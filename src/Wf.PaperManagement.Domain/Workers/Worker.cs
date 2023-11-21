using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace csuwf.PaperManagement.Workers;

public class Worker : FullAuditedEntity
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 工号
    /// </summary>
    public int WorkerId { get; set; }

    public Guid UserId { get; set; }

    protected Worker()
    {
    }

    public Worker(Guid userId, int workerId, string name)
    {
        UserId = userId;
        WorkerId = workerId;
        Name = name;
    }

    public void SetName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name), WorkerConsts.MaxNameLength);
        Name = name;
    }

    public override object?[] GetKeys()
    {
        return new object[] { WorkerId, UserId };
    }
}