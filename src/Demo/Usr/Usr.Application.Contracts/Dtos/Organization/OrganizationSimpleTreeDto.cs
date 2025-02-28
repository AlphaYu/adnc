namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 精简部门树结构
/// </summary>
[Serializable]
public class OrganizationSimpleTreeDto : OutputDto
{
    /// <summary>
    /// 唯一Id
    /// </summary>
    public override long Id { get; set; }

    /// <summary>
    /// 部门简称
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// 子部门
    /// </summary>
    public List<OrganizationSimpleTreeDto> Children { get; private set; } = new List<OrganizationSimpleTreeDto>();
}