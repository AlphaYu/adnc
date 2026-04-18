namespace Adnc.Demo.Ord.Application.Contracts.Dtos.Order;

public class OrderUpdationDto : IDto
{
    /// <summary>
    /// Receiver information
    /// </summary>
    public OrderReceiverDto DeliveryInfomaton { get; set; } = default!;
}
