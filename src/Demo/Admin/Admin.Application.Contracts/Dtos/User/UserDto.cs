namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 用户
/// </summary>
[Serializable]
public class UserDto : UserCreationAndUpdationDto
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间/注册时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}
