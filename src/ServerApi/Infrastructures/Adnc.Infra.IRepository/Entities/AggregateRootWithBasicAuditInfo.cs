using System;

namespace Adnc.Infra.Entities
{
    public class AggregateRootWithBasicAuditInfo : AggregateRoot, IBasicAuditInfo
    {
        public long CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
    }
}