namespace Adnc.Demo.Maint.Application.Dtos;

/// <summary>
/// 系统配置
/// </summary>
public class CfgCreationDto : InputDto
{
    /// <summary>
    /// 备注
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 参数名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 参数值
    /// </summary>
    public string Value { get; set; } = string.Empty;
}