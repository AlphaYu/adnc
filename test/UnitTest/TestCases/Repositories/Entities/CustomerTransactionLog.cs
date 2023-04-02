using Adnc.Infra.Entities;

namespace Adnc.UnitTest.TestCases.Repositories.Entities;

/// <summary>
/// 客户财务变动记录
/// </summary>
public class CustomerTransactionLog : EfBasicAuditEntity
{
    public long CustomerId { get; set; }

    public string Account { get; set; }

    public ExchangeBehavior ExchangeType { get; set; }

    public ExchageStatus ExchageStatus { get; set; }

    public decimal ChangingAmount { get; set; }

    public decimal Amount { get; set; }

    public decimal ChangedAmount { get; set; }

    public string Remark { get; set; }
}

public enum ExchangeBehavior
{
    Recharge = 8000
    ,
    Order = 8008
}

public enum ExchageStatus
{
    Processing = 2000
    ,
    Finished = 2008
    ,
    Failed = 2016
}