namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Menu;

/// <summary>
/// Represents the payload used to create a menu.
/// </summary>
public class MenuCreationDto : InputDto
{
    /// <summary>
    /// Gets or sets the parent menu ID.
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// Gets or sets the menu name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the permission code.
    /// </summary>
    public string Perm { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the route name.
    /// </summary>
    public string RouteName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the route path.
    /// </summary>
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the menu type.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the component path.
    /// </summary>
    public string Component { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the menu is visible.
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Gets or sets the redirect route path.
    /// </summary>
    public string Redirect { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the icon.
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether page caching is enabled.
    /// </summary>
    public bool KeepAlive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a single child route should always be shown.
    /// </summary>
    public bool AlwaysShow { get; set; }

    /// <summary>
    /// Gets or sets the route parameters.
    /// </summary>
    public List<KeyValuePair<string, string>> Params { get; set; } = [];

    /// <summary>
    /// Gets or sets the display order.
    /// </summary>
    public int Ordinal { get; set; }
}
