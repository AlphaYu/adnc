namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 系统配置
/// </summary>
public class SysConfigSearchPagedDto : SearchPagedDto
{
    /// <summary>
    /// 参数键/名称
    /// </summary>
    public string? Keywords { get; set; }
}