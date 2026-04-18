namespace Adnc.Infra.Repository;

public interface IFullAuditInfo : IBasicAuditInfo
{
    /// <summary>
    /// Last updater
    /// </summary>
    public long ModifyBy { get; set; }

    /// <summary>
    /// Last update time
    /// </summary>
    public DateTime ModifyTime { get; set; }
}
