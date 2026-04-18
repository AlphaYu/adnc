namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Menu;

/// <summary>
/// Represents a menu.
/// </summary>
[Serializable]
public class MenuDto : MenuCreationDto
{
    /// <summary>
    /// Gets or sets the menu ID.
    /// </summary>
    public long Id { get; set; }
}
