using Adnc.Core.Shared.Entities;

namespace Adnc.Usr.Core.Entities
{
    /// <summary>
    /// 菜单角色关系
    /// </summary>
    public class SysRelation : EfEntity
    {
        public long MenuId { get; set; }

        public long RoleId { get; set; }

        public virtual SysRole Role { get; set; }

        public virtual SysMenu Menu { get; set; }
    }
}