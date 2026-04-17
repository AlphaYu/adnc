namespace Adnc.Demo.Admin.Repository.Entities;

/// <summary>
/// 角色
/// </summary>
public class Role : EfFullAuditEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public int DataScope { get; set; }

    public bool Status { get; set; }

    public int Ordinal { get; set; }
}
