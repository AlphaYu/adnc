namespace Adnc.Infra.Entities
{
    public abstract class EfFullAuditEntity : EfEntity, IFullAuditInfo
    {
        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateBy { get; set; }

        /// <summary>
        /// 创建时间/注册时间
        /// </summary>
        public DateTime CreateTime { get; set; }

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