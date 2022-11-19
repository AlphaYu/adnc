namespace Adnc.Shared.Domain.Entities;

public class AggregateRootWithBasicAuditInfo : AggregateRoot, IBasicAuditInfo
{
    public long CreateBy { get; set; }
    public DateTime CreateTime { get; set; }
}