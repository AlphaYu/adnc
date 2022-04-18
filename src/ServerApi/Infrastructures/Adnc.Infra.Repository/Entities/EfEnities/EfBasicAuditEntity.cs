namespace Adnc.Infra.Entities
{
    public abstract class EfBasicAuditEntity : EfEntity, IBasicAuditInfo
    {
        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateBy { get; set; }

        /// <summary>
        /// 创建时间/注册时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}