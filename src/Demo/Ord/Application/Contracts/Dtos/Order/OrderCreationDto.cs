namespace Adnc.Demo.Ord.Application.Contracts.Dtos.Order;

public class OrderCreationDto : IDto
{
    /// <summary>
    /// Customer ID
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// Receiver information
    /// </summary>
    public OrderReceiverDto DeliveryInfomaton { get; set; } = default!;

    /// <summary>
    /// Order items
    /// </summary>
    public virtual ICollection<OrderCreationItemDto> Items { get; set; } = [];

    public class OrderCreationItemDto : IDto
    {
        public long ProductId { get; set; }

        public int Count { get; set; }
    }
}
