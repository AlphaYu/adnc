namespace Adnc.Infra.Unittest.Reposity.Fixtures.Entities;

/// <summary>
/// 客户表
/// </summary>
public class Customer : EfFullAuditEntity
{
    public string Account { get; set; }

    public string Password { get; set; } = string.Empty;

    public string Nickname { get; set; }

    public string Realname { get; set; }

    public virtual CustomerFinance FinanceInfo { get; set; }

    public virtual ICollection<CustomerTransactionLog> TransactionLogs { get; set; }
}
