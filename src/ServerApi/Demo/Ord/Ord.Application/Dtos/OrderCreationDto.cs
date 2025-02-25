namespace Adnc.Demo.Ord.Application.Dtos;

public class OrderCreationDto : IDto
{
    /// <summary>
    /// 客户Id
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 收货信息
    /// </summary>
    public OrderReceiverDto DeliveryInfomaton { get; set; }

    /// <summary>
    /// 订单子项
    /// </summary>
    public virtual ICollection<OrderCreationItemDto> Items { get; set; }

    public class OrderCreationItemDto : IDto
    {
        public long ProductId { get; set; }

        public int Count { get; set; }
    }
}