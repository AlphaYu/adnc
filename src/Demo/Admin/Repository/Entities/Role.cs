namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// Role
/// </summary>
public class Role : EfFullAuditEntity
{
    public static readonly int Name_MaxLength = 32;
    public static readonly int Code_MaxLength = 32;

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public int DataScope { get; set; }

    public bool Status { get; set; }

    public int Ordinal { get; set; }
}
