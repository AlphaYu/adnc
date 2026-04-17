namespace Adnc.Demo.Ord.Application.Contracts.Dtos.Order;

public class OrderMarkCreatedStatusDto
{
    public bool IsSuccess { get; set; }

    public string Remark { get; set; } = string.Empty;
}
