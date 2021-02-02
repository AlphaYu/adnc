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
        [StringLength(64)]
        [Column("CfgName")]
        public string CfgName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [Description("参数值")]
        [StringLength(128)]
        [Column("CfgValue")]
        public string CfgValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [StringLength(256)]
        [Column("CfgDesc")]
        public string CfgDesc { get; set; }

        public bool IsDeleted { get; set; }
    }
}
