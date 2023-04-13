namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 路由菜单
/// </summary>
[Serializable]
public class MenuRouterDto : OutputDto
{
    /// <summary>
    /// 父菜单Id
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 菜编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 父菜单编码
    /// </summary>
    public string PCode { get; set; }

    /// <summary>
    /// 菜单路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 菜单对应视图组件
    /// </summary>
    public string Component { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    public int Ordinal { get; set; }

    /// <summary>
    /// 是否隐藏
    /// </summary>
    public bool Hidden { get; set; } = false;

    /// <summary>
    /// 菜单元数据
    /// </summary>
    public MenuMetaDto Meta { get; set; }

    /// <summary>
    /// 子菜单
    /// </summary>
    public List<MenuRouterDto> Children { get; private set; } = new List<MenuRouterDto>();
}