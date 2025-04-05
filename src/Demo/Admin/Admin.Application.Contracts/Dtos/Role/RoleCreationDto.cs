namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

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
    /// 角色状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public int DataScope { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    public int Ordinal { get; set; }
}
