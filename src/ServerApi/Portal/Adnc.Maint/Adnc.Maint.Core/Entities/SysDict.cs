using System.ComponentModel;
using Adnc.Core.Shared.Entities;

namespace Adnc.Maint.Core.Entities
{
    /// <summary>
    /// 字典
    /// </summary>
    public class SysDict : EfFullAuditEntity, ISoftDelete
    {
        public string Name { get; set; }

        public int Ordinal { get; set; }

        public long Pid { get; set; }

        public string Value { get; set; }

        public bool IsDeleted { get; set; }
    }
}
