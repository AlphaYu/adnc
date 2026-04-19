namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// Menu
/// </summary>
public class Menu : EfFullAuditEntity
{
    public static readonly int Code_MaxLength = 32;
    public static readonly int ParentIds_MaxLength = 128;
    public static readonly int Component_MaxLength = 64;
    public static readonly int Icon_MaxLength = 32;
    public static readonly int Name_MaxLength = 32;
    public static readonly int RouteName_MaxLength = 64;
    public static readonly int RoutePath_MaxLength = 64;
    public static readonly int Redirect_MaxLength = 128;
    public static readonly int Title_MaxLength = 16;
    public static readonly int Type_MaxLength = 16;
    public static readonly int Params_MaxLength = 128;

    /// <summary>
    /// Parent menu ID
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// Parent menu ID path
    /// </summary>
    public string ParentIds { get; set; } = string.Empty;

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Permission code
    /// </summary>
    public string Perm { get; set; } = string.Empty;

    /// <summary>
    /// Route name
    /// </summary>
    public string RouteName { get; set; } = string.Empty;

    /// <summary>
    /// Route path
    /// </summary>
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// Menu type
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Component configuration
    /// </summary>
    public string Component { get; set; } = string.Empty;

    /// <summary>
    /// Visible
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Redirect route path
    /// </summary>
    public string Redirect { get; set; } = string.Empty;

    /// <summary>
    /// Icon
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Enable page caching
    /// </summary>
    public bool KeepAlive { get; set; }

    /// <summary>
    /// Always show when there is only one child route
    /// </summary>
    public bool AlwaysShow { get; set; }

    /// <summary>
    /// Route parameters
    /// </summary>
    public string Params { get; set; } = string.Empty;

    /// <summary>
    /// Ordinal
    /// </summary>
    public int Ordinal { get; set; }
}
