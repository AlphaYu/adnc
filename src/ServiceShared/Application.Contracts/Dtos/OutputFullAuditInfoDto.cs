namespace Adnc.Shared.Application.Contracts.Dtos;

/// <summary>
/// Dto基类
/// </summary>
[Serializable]
public abstract class OutputFullAuditInfoDto : OutputBaseAuditDto
{
    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual long ModifyBy { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    public virtual DateTime ModifyTime { get; set; }
}
