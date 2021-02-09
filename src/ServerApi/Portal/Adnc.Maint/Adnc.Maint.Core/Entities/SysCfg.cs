using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adnc.Core.Shared.Entities;

namespace Adnc.Maint.Core.Entities
{
    /// <summary>
    /// 系统参数
    /// </summary>
    [Table("SysCfg")]
    [Description("系统参数")]
    public class SysCfg : EfFullAuditEntity, ISoftDelete
    {
        /// <summary>
        /// 参数名
        /// </summary>
        [Description("参数名")]
        [MaxLength(64)]
        [Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [Description("参数值")]
        [MaxLength(128)]
        [Column("Value")]
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [MaxLength(256)]
        [Column("Description")]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
    }
}
