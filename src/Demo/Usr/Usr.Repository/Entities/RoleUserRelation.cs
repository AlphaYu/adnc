namespace Adnc.Demo.Usr.Repository.Entities;

/// <summary>
/// 用户角色关系
/// </summary>
public class RoleUserRelation : EfEntity
{
    public long UserId { get; set; }

    public long RoleId { get; set; }
}