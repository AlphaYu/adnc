namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 角色
/// </summary>
[Serializable]
public class RoleDto : RoleCreationDto
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public long Id { get; set; }
}
