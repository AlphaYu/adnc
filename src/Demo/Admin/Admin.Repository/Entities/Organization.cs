namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// 部门
/// </summary>
public class Organization : EfFullAuditEntity
{
    public long ParentId { get; set; }

    public string ParentIds { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool Status { get; set; }

    public int Ordinal { get; set; }
}
