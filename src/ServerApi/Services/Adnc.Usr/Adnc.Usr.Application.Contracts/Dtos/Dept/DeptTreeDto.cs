namespace Adnc.Usr.Application.Contracts.Dtos;

/// <summary>
/// 部门节点
/// </summary>
[Serializable]
public class DeptTreeDto : DeptDto
{
    public List<DeptTreeDto> Children { get; set; } = new List<DeptTreeDto>();
}