namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 系统配置
/// </summary>
[Serializable]
public class SysConfigSimpleDto
{
    /// <summary>
    /// 参数键
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// 参数名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 参数值
    /// </summary>
    public string Value { get; set; } = string.Empty;
}
