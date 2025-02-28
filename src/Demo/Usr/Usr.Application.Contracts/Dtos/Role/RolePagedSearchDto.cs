namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 角色检索条件
/// </summary>
public class RolePagedSearchDto : SearchPagedDto
{
    /// <summary>
    /// 角色名
    /// </summary>
    public string? RoleName { get; set; }
}