namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// 菜单角色关系
/// </summary>
public class RoleMenuRelation : EfEntity
{
    public long MenuId { get; set; }

    public long RoleId { get; set; }
}
