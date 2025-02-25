namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 部门节点
/// </summary>
[Serializable]
public class OrganizationTreeDto : OrganizationDto
{
    public List<OrganizationTreeDto> Children { get; set; } = new List<OrganizationTreeDto>();
}