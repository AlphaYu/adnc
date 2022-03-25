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
    public List<DeptTreeDto> Children { get; private set; } = new List<DeptTreeDto>();
}