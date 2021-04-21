using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Core.Shared.Entities
{
    public interface IFullAuditInfo: IBasicAuditInfo
    {
        /// <summary>
        /// 最后更新人
        /// </summary>
        public long? ModifyBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}
