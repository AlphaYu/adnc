namespace Adnc.Infra.Unittest.Reposity.Fixtures.Entities;

/// <summary>
/// 工程表，仅用于测试，无实际意义
/// </summary>
public class Project : EfFullAuditEntity, ISoftDelete, IConcurrency
{
    public string Name { get; set; }

    public bool IsDeleted { get; set; }

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
