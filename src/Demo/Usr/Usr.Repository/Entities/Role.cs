namespace Adnc.Demo.Usr.Repository.Entities;

/// <summary>
/// 角色
/// </summary>
public class Role : EfFullAuditEntity
{
    public long DeptId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Ordinal { get; set; }

    public long Pid { get; set; } = 0;

    public string Code { get; set; } = string.Empty;
}