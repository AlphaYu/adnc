using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adnc.Core.Shared.Entities;

namespace Adnc.Maint.Core.Entities
{
    /// <summary>
    /// 字典
    /// </summary>
    [Table("SysDict")]
    [Description("字典")]
    public class SysDict : EfFullAuditEntity, ISoftDelete
    {
        [MaxLength(16)]
        public string Name { get; set; }

        [Description("排序号")]
        public int Ordinal { get; set; }

        [Description("父节点Id")]
        public long Pid { get; set; }

        [MaxLength(64)]
        public string Value { get; set; }

        public bool IsDeleted { get; set; }
    }
}
