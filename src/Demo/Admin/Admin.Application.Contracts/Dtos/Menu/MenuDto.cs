namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 菜单
/// </summary>
[Serializable]
public class MenuDto : MenuCreationDto
{
    /// <summary>
    /// 菜单Id
    /// </summary>
    public long Id { get; set; }
}
