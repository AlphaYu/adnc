namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// 菜单
/// </summary>
public class Menu : EfFullAuditEntity
{
    /// <summary>
    /// 父菜单Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 父菜单Id组合
    /// </summary>
    public string ParentIds { get; set; } = string.Empty;

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 权限编码
    /// </summary>
    public string Perm { get; set; } = string.Empty;

    /// <summary>
    /// 路由名称
    /// </summary>
    public string RouteName { get; set; } = string.Empty;

    /// <summary>
    /// 路由路径
    /// </summary>
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 菜单类型
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 組件配置
    /// </summary>
    public string Component { get; set; } = string.Empty;

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// 跳转路由路径
    /// </summary>
    public string Redirect { get; set; } = string.Empty;

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 是否开启页面缓存
    /// </summary>
    public bool KeepAlive { get; set; }

    /// <summary>
    /// 只有一个子路由是否始终显示
    /// </summary>
    public bool AlwaysShow { get; set; }

    /// <summary>
    ///  路由参数
    /// </summary>
    public string Params { get; set; } = string.Empty;

    /// <summary>
    /// 序号
    /// </summary>
    public int Ordinal { get; set; }
}
