namespace Adnc.Infra.Repository;

public abstract class EfFullAuditEntity : EfEntity, IFullAuditInfo
{
    /// <summary>
    /// Creator
    /// </summary>
    public long CreateBy { get; set; }

    /// <summary>
    /// Creation time / registration time
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Last modifier
    /// </summary>
    public long ModifyBy { get; set; }

    /// <summary>
    /// Last modification time
    /// </summary>
    public DateTime ModifyTime { get; set; }
}
