namespace Adnc.Demo.Cust.Api.Repository.Entities;

/// <summary>
/// 客户财务表
/// </summary>
public class CustomerFinance : EfFullAuditEntity, IConcurrency
{
    public string Account { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public virtual Customer Customer { get; set; } = default!;

    public byte[] RowVersion { get; set; } = default!;
}