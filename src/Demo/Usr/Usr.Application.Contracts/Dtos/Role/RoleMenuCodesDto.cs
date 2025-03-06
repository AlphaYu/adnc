namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 角色，权限
/// </summary>
[Serializable]
public class RoleMenuCodesDto : IDto
{
    /// <summary>
    /// 菜单Code
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 角色Id
    /// </summary>
    public long RoleId { get; set; }
}