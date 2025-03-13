namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 角色检索条件
/// </summary>
public class RolePagedSearchDto : SearchPagedDto
{
    /// <summary>
    /// 角色名
    /// </summary>
    public string? Keywords { get; set; }
}