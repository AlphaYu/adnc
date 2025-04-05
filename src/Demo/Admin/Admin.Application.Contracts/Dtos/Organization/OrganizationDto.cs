namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 部门
/// </summary>
[Serializable]
public class OrganizationDto : OrganizationCreationDto
{
    /// <summary>
    /// 部门Id
    /// </summary>
    public long Id { get; set; }
}
