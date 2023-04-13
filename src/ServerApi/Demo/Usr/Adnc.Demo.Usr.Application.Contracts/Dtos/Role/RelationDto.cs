namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 菜单-角色关联
/// </summary>
[Serializable]
public class RelationDto : IDto
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    public long? MenuId { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    public long? RoleId { get; set; }
}