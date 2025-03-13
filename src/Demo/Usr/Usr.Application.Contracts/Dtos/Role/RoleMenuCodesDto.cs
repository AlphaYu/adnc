namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 角色，权限
/// </summary>
[Serializable]
public class RoleMenuCodesDto : IDto
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 菜单Id
    /// </summary>
    public long MenuId { get; set; }

    /// <summary>
    /// 权限代码
    /// </summary>
    public string MenuPerm { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名
    /// </summary>
    public string MenuName { get; set; } = string.Empty;
}