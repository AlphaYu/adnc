namespace Adnc.Demo.Usr.Repository.Entities;

/// <summary>
/// 部门
/// </summary>
public class Organization : EfFullAuditEntity
{
    public string FullName { get; set; } = string.Empty;

    public int Ordinal { get; set; }

    public long? Pid { get; set; }

    public string Pids { get; set; } = string.Empty;

    public string SimpleName { get; set; } = string.Empty;

    public string? Tips { get; set; } = string.Empty;
}