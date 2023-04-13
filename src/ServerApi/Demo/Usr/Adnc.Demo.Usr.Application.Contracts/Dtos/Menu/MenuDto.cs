namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 菜单
/// </summary>
[Serializable]
public class MenuDto : OutputDto
{
    /// <summary>
    /// 编号
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 組件配置
    /// </summary>
    public string Component { get; set; } = string.Empty;

    /// <summary>
    /// 是否隐藏
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 是否是菜单1:菜单,0:按钮
    /// </summary>
    public bool IsMenu { get; set; }

    /// <summary>
    /// 是否默认打开1:是,0:否
    /// </summary>
    public bool IsOpen { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    public int Levels { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 序号
    /// </summary>
    public int Ordinal { get; set; }

    /// <summary>
    /// 父菜单编号
    /// </summary>
    public string PCode { get; set; } = string.Empty;

    /// <summary>
    /// 递归父级菜单编号
    /// </summary>
    public string PCodes { get; set; } = string.Empty;

    /// <summary>
    /// 状态1:启用,0:禁用
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 鼠标悬停提示信息
    /// </summary>
    public string Tips { get; set; } = string.Empty;

    /// <summary>
    /// 链接
    /// </summary>
    public string Url { get; set; } = string.Empty;
}