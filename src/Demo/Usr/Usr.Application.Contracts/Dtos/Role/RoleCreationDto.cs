namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

public class RoleCreationDto : InputDto
{
    /// <summary>
    /// 角色名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 角色代码
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 序号
    /// </summary>
    public int Ordinal { get; set; }
}