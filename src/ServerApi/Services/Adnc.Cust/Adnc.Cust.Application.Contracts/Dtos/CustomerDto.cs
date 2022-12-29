namespace Adnc.Cust.Application.Contracts.Dtos;

public class CustomerDto : OutputBaseAuditDto
{
    public string Account { get; set; }

    public string Nickname { get; set; }

    public string Realname { get; set; }

    public decimal FinanceInfoBalance { get; set; }
}