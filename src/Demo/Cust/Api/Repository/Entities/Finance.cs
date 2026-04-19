namespace Adnc.Demo.Cust.Api.Repository.Entities;

/// <summary>
/// Customer finance table
/// </summary>
public class Finance : EfFullAuditEntity, IConcurrency
{
    public static readonly int Account_MaxLength = 16;

    public string Account { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public byte[] RowVersion { get; set; } = default!;
}
