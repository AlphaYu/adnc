namespace Adnc.Demo.Maint.Repository.Entities;

/// <summary>
/// 系统参数
/// </summary>
public class Cfg : EfFullAuditEntity, ISoftDelete
{
    /// <summary>
    /// 参数名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 参数值
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    public string Description { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}