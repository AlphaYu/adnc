namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

public class RoleCreationDto : InputDto
{
    /// <summary>
    /// 角色名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 角色描述
    /// </summary>
    public string Tips { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    public int Ordinal { get; set; }
}