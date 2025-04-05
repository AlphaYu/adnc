namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 用户检索条件
/// </summary>
public class UserSearchPagedDto : SearchPagedDto
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public bool? Status { get; set; }

    /// <summary>
    /// 部门编号
    /// </summary>
    public long? DeptId { get; set; }
}
