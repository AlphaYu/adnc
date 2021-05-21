using System;

namespace Adnc.Application.Shared.Dtos
{
    /// <summary>
    /// DTO基类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
   	[Serializable]
    public abstract class OutputFullAuditInfoDto<TKey> : OutputBaseAuditDto<TKey>, IFullAuditInfo, IOutputDto<TKey>
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