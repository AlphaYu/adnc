using System.Collections.ObjectModel;
using Adnc.Core.Shared.Entities;

namespace Adnc.Usr.Core.Entities
{
    /// <summary>
    /// 角色
    /// </summary>
    public class SysRole : EfFullAuditEntity
    {
        public long? DeptId { get; set; }

        public string Name { get; set; }

        public int Ordinal { get; set; }

        public long? Pid { get; set; }

        public string Tips { get; set; }

        public int? Version { get; set; }

        public virtual Collection<SysRelation> Relations { get; set; }
    }
}
