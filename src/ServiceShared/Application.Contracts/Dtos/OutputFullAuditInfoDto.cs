namespace Adnc.Shared.Application.Contracts.Dtos;

/// <summary>
/// Base DTO
/// </summary>
[Serializable]
public abstract class OutputFullAuditInfoDto : OutputBaseAuditDto
{
    /// <summary>
    /// Last modified by
    /// </summary>
    public virtual long ModifyBy { get; set; }

    /// <summary>
    /// Last modified time
    /// </summary>
    public virtual DateTime ModifyTime { get; set; }
}
