namespace Adnc.Infra.Unittest.Reposity.Fixtures.Entities;

/// <summary>
/// Project table, used for testing only with no real business meaning
/// </summary>
public class Project : EfFullAuditEntity, ISoftDelete, IConcurrency
{
    public string Name { get; set; }

    public bool IsDeleted { get; set; }

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
