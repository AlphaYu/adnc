namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// 管理员
/// </summary>
public class User : EfFullAuditEntity, ISoftDelete
{
    //private SysDept _dept;
    //private Action<object, string> LazyLoader { get; set; }
    //private SysUser(Action<object, string> lazyLoader)
    //{
    //	LazyLoader = lazyLoader;
    //}
    //public virtual SysDept Org
    //{
    //   get => LazyLoader.Load(this, ref _dept);
    //   set => _dept = value;
    //}

    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 头像路径
    /// </summary>
    public string Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    public long DeptId { get; set; }

    /// <summary>
    /// email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 密码盐
    /// </summary>
    public string Salt { get; set; } = string.Empty;

    /// <summary>
    /// 性别
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 删除标识
    /// </summary>

    public bool IsDeleted { get; set; }
}
