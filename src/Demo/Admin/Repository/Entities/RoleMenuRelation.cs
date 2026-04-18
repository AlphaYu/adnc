namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// Menu-role relation
/// </summary>
public class RoleMenuRelation : EfEntity
{
    public long MenuId { get; set; }

    public long RoleId { get; set; }
}
