namespace Adnc.Shared.Application.Contracts.Dtos;

[Serializable]
public abstract class OutputBaseAuditDto : OutputDto
{
    /// <summary>
    /// Created by
    /// </summary>
    public virtual long CreateBy { get; set; }

    /// <summary>
    /// Creation time / registration time
    /// </summary>
    public virtual DateTime CreateTime { get; set; }
}
