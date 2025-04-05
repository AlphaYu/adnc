namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 角色，权限
/// </summary>
[Serializable]
public class RoleMenuCodeDto : IDto
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 权限代码
    /// </summary>
    public string[] Perms { get; set; } = [];
}
