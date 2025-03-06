namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 角色
/// </summary>
[Serializable]
public class RoleDto : OutputDto
{
    ///// <summary>
    ///// 部门Id
    ///// </summary>
    ////public long? DeptId { get; set; }

    /// <summary>
    /// 角色名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 序号
    /// </summary>
    public int Ordinal { get; set; }

    /// <summary>
    /// 父级角色Id
    /// </summary>
    public long Pid { get; set; }

    /// <summary>
    /// 角色代码
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 权限集合
    /// </summary>
    public string Permissions { get; set; } = string.Empty;
}