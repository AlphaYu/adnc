namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 用户检索条件
/// </summary>
public class UserSearchPagedDto : SearchPagedDto
{
    /// <summary>
    /// 用户姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 用户账户
    /// </summary>
    public string? Account { get; set; }
}