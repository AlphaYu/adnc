namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 部门树
/// </summary>
[Serializable]
public class OrganizationTreeDto : OrganizationDto
{
    public List<OrganizationTreeDto> Children { get; set; } = [];
}
