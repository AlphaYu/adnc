namespace Adnc.Demo.Maint.Repository.Entities;

/// <summary>
/// 字典
/// </summary>
public class Dict : EfFullAuditEntity, ISoftDelete
{
    public string Name { get; set; } = string.Empty;

    public int Ordinal { get; set; }

    public long Pid { get; set; }

    public string Value { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}