namespace Adnc.Demo.Cust.Api.Application.Dtos;

public class TransactionLogDto : OutputDto
{
    public long CustomerId { get; set; }

    public string Account { get; set; } = string.Empty;

    public int ExchangeType { get; set; }

    public string ExchangeTypeName { get; set; } = string.Empty;

    public int ExchageStatus { get; set; }

    public string ExchageStatusName { get; set; } = string.Empty;

    public decimal ChangingAmount { get; set; }

    public decimal Amount { get; set; }

    public decimal ChangedAmount { get; set; }

    public string Remark { get; set; } = string.Empty;

    public DateTime CreateTime { get; set; }
}
