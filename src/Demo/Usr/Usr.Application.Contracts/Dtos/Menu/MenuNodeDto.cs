namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 菜单节点
/// </summary>
[Serializable]
public class MenuNodeDto : OutputDto
{
    /// <summary>
    /// 父菜单Id
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 菜单级别
    /// </summary>
    public int Levels { get; set; }

    /// <summary>
    /// 是否是菜单（菜单还是按钮）
    /// </summary>
    public bool IsMenu { get; set; }

    /// <summary>
    /// 是否菜单
    /// </summary>
    public string IsMenuName => IsMenu ? "是" : "否";

    /// <summary>
    /// 菜单状态
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 菜单状态
    /// </summary>
    public string StatusName => Status == 1 ? "启用" : "禁用";

    /// <summary>
    /// 序号
    /// </summary>
    public int Ordinal { get; set; }

    /// <summary>
    /// 菜单URL
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 菜单路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 菜单图标
    /// </summary>
    private string _icon;

    public string Icon
    {
        set
        {
            _icon = value;
        }
        get
        {
            return _icon ?? string.Empty;
        }
    }

    /// <summary>
    /// 菜单编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 父菜单编码
    /// </summary>
    public string PCode { get; set; }

    /// <summary>
    /// 菜单对饮视图组件
    /// </summary>
    public string Component { get; set; }

    /// <summary>
    /// 是否隐藏
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// 子菜单
    /// </summary>
    public List<MenuNodeDto> Children { get; private set; } = new List<MenuNodeDto>();
}