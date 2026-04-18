namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// User-role relation
/// </summary>
public class RoleUserRelation : EfEntity
{
    public long UserId { get; set; }

    public long RoleId { get; set; }
}
