using System.Text.Json.Serialization;

namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Menu;

/// <summary>
/// Represents a menu tree node.
/// </summary>
[Serializable]
public class MenuTreeDto : MenuDto
{
    /// <summary>
    /// Gets or sets the child menus.
    /// </summary>
    [JsonPropertyOrder(100)]
    public List<MenuTreeDto> Children { get; set; } = [];
}
