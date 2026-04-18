namespace Adnc.Infra.Repository;

public abstract class EfBasicAuditEntity : EfEntity, IBasicAuditInfo
{
    /// <summary>
    /// Creator
    /// </summary>
    public long CreateBy { get; set; }

    /// <summary>
    /// Creation time / registration time
    /// </summary>
    public DateTime CreateTime { get; set; }
}
