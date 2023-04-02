using Adnc.Infra.Entities;

namespace Adnc.UnitTest.TestCases.Repositories.Entities;

/// <summary>
/// 客户财务表
/// </summary>
public class CustomerFinance : EfFullAuditEntity, IConcurrency
{
    public string Account { get; set; }

    public decimal Balance { get; set; }

    public virtual Customer Customer { get; set; }

    public byte[] RowVersion { get; set; }
}