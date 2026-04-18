namespace Adnc.Demo.Cust.Api.Repository.Entities;

/// <summary>
/// Customer table
/// </summary>
public class Customer : EfFullAuditEntity
{
    public string Account { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Nickname { get; set; } = string.Empty;

    public string Realname { get; set; } = string.Empty;

    public virtual required Finance FinanceInfo { get; set; }

    public virtual ICollection<TransactionLog>? TransactionLogs { get; set; }
}
