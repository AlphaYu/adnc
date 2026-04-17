namespace Adnc.Demo.Ord.Application.Contracts.Dtos.Order;

public class OrderUpdationDto : IDto
{
    /// <summary>
    /// 收货信息
    /// </summary>
    public OrderReceiverDto DeliveryInfomaton { get; set; } = default!;
}
