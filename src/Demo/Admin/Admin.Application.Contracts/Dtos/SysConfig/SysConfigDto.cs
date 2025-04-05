namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 系统配置
/// </summary>
[Serializable]
public class SysConfigDto : SysConfigCreationDto
{
    /// <summary>
    /// 参数Id
    /// </summary>
    public long Id { get; set; }
}
