namespace Adnc.Usr.Application.Contracts.Dtos;

/// <summary>
/// 部门节点
/// </summary>
[Serializable]
public class DeptTreeDto : DeptDto
{
    /// <summary>
    /// 子部门
    /// </summary>
    private List<DeptTreeDto> _children;

    public List<DeptTreeDto> Children { get; set; } = new List<DeptTreeDto>();
}