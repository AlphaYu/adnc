namespace Adnc.Demo.Ord.Application.Dtos;

public class OrderUpdationDto : IDto
{
    /// <summary>
    /// 收货信息
    /// </summary>
    public OrderReceiverDto DeliveryInfomaton { get; set; }
}