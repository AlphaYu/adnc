namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 部门
/// </summary>
public class OrganizationCreationDto : InputDto
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 部门编号
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 部门全称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 部门状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    public int Ordinal { get; set; }
}
