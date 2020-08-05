using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
    public abstract class EfAuditEntity : EfEntity, IAudit
    {
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
		[Column("CreateBy")]
		public virtual long? CreateBy { get; set; }

		/// <summary>
		/// 创建时间/注册时间
		/// </summary>
		[Description("创建时间/注册时间")]
		[Column("CreateTime")]
		public virtual DateTime? CreateTime { get; set; }

		/// <summary>
		/// 最后更新人
		/// </summary>
		[Description("最后更新人")]
		[Column("ModifyBy")]
		public virtual long? ModifyBy { get; set; }

		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		[Column("ModifyTime")]
		public virtual DateTime? ModifyTime { get; set; }
	}
}
