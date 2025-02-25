namespace Adnc.Demo.Maint.Application.Dtos;

/// <summary>
/// 系统配置
/// </summary>
public class CfgSearchPagedDto : SearchPagedDto
{
    /// <summary>
    /// 参数名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 参数值
    /// </summary>
    public string? Value { get; set; }
}