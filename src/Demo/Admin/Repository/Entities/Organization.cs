namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// Organization
/// </summary>
public class Organization : EfFullAuditEntity
{
    public static readonly int Name_MaxLength = 32;
    public static readonly int Code_MaxLength = 16;
    public static readonly int Pids_MaxLength = 128;

    public long ParentId { get; set; }

    public string ParentIds { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool Status { get; set; }

    public int Ordinal { get; set; }
}
