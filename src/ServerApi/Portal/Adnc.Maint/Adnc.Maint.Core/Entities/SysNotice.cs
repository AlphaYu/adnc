using Adnc.Core.Shared.Entities;

namespace Adnc.Maint.Core.Entities
{
    /// <summary>
    /// 通知
    /// </summary>
    public class SysNotice : EfFullAuditEntity
    {
        public string Content { get; set; }

        public string Title { get; set; }

        public int? Type { get; set; }
    }
}
