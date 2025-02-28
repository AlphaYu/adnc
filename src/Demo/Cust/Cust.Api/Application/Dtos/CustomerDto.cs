namespace Adnc.Demo.Cust.Api.Application.Dtos;

public class CustomerDto : OutputBaseAuditDto
{
    public string Account { get; set; } = string.Empty;

    public string Nickname { get; set; } = string.Empty;

    public string Realname { get; set; } = string.Empty;

    public decimal FinanceInfoBalance { get; set; }
}