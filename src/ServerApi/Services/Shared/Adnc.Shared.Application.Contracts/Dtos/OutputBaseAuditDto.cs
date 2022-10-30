namespace Adnc.Shared.Application.Contracts.Dtos;

[Serializable]
public abstract class OutputBaseAuditDto : OutputDto
{
    /// <summary>
    /// 创建人
    /// </summary>
    public virtual long? CreateBy { get; set; }

    /// <summary>
    /// 创建时间/注册时间
    /// </summary>
    public virtual DateTime? CreateTime { get; set; }
}