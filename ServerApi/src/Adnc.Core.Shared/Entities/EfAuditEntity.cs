using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Shared.Entities
{
    public abstract class EfAuditEntity : EfEntity, IAudit
    {
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
