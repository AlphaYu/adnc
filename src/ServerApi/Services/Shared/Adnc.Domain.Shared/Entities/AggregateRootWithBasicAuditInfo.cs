namespace Adnc.Domain.Shared.Entities;

public class AggregateRootWithBasicAuditInfo : AggregateRoot, IBasicAuditInfo
{
    public long CreateBy { get; set; }
    public DateTime CreateTime { get; set; }
}