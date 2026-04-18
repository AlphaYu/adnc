using System.Text.Json.Serialization;

namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Menu;

/// <summary>
/// Represents a router tree node returned to the client.
/// </summary>
public sealed class RouterTreeDto
{
    /// <summary>
    /// Gets or sets the route name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the route path.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the component path.
    /// </summary>
    public string Component { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the redirect route path.
    /// </summary>
    public string Redirect { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the route metadata.
    /// </summary>
    public required RouteMeta Meta { get; set; }

    /// <summary>
    /// Gets or sets the child routes.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<RouterTreeDto>? Children { get; set; }

    /// <summary>
    /// Represents route metadata.
    /// </summary>
    public sealed class RouteMeta
    {
        /// <summary>
        /// Gets or sets the route title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the route icon.
        /// </summary>
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the route is hidden.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the route should always be shown.
        /// </summary>
        public bool AlwaysShow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the route should be kept alive.
        /// </summary>
        public bool KeepAlive { get; set; }
    }
}
