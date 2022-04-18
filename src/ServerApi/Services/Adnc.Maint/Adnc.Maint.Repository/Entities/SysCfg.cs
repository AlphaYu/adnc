using Adnc.Infra.Entities;

namespace Adnc.Maint.Entities
{
    /// <summary>
    /// 系统参数
    /// </summary>
    public class SysCfg : EfFullAuditEntity, ISoftDelete
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
    }
}