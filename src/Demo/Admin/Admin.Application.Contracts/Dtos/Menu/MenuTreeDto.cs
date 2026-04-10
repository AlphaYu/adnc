using System.Text.Json.Serialization;

namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Menu;

/// <summary>
/// 菜单
/// </summary>
[Serializable]
public class MenuTreeDto : MenuDto
{
    /// <summary>
    /// 子菜单
    /// </summary>
    [JsonPropertyOrder(100)]
    public List<MenuTreeDto> Children { get; set; } = [];
}
