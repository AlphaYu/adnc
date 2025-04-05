namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

public abstract class UserCreationAndUpdationDto : InputDto
{
    ///// <summary>
    ///// 头像
    ///// </summary>
    ////public string Avatar { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    public long DeptId { get; set; }

    /// <summary>
    /// 邮件地址
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    // <summary>
    // 角色Id
    // </summary>
    public long[] RoleIds { get; set; } = [];

    /// <summary>
    /// 性别
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// 账户状态
    /// </summary>
    public bool Status { get; set; }
}
