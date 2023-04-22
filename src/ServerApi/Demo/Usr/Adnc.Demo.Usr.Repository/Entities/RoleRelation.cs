namespace Adnc.Demo.Usr.Repository.Entities;

/// <summary>
/// 菜单角色关系
/// </summary>
public class RoleRelation : EfEntity
{
    public long MenuId { get; set; }

    public long RoleId { get; set; }

    public virtual Menu Menu { get; set; } = default!;
}