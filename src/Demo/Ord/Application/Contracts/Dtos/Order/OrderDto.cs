namespace Adnc.Demo.Ord.Application.Contracts.Dtos.Order;

/// <summary>
/// Order output DTO
/// </summary>
public class OrderDto : OutputBaseAuditDto
{
    /// <summary>
    /// Customer ID
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// Order amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// Order status - status code
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Order status - status name
    /// </summary>
    public string StatusName { get; set; } = string.Empty;

    /// <summary>
    /// Order status - status change reason
    /// </summary>
    public string StatusChangesReason { get; set; } = string.Empty;

    /// <summary>
    /// Receiver information - recipient
    /// </summary>
    public string ReceiverName { get; set; } = string.Empty;

    /// <summary>
    /// Receiver information - phone
    /// </summary>
    public string ReceiverPhone { get; set; } = string.Empty;

    /// <summary>
    /// Receiver information - address
    /// </summary>
    public string ReceiverAddress { get; set; } = string.Empty;

    /// <summary>
    /// Order items
    /// </summary>
    public virtual ICollection<OrderItemDto> Items { get; set; } = [];

    /// <summary>
    /// Order item DTO
    /// </summary>
    public class OrderItemDto : OutputDto
    {
        /// <summary>
        /// Order ID
        /// </summary>
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// Quantity
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Product information - product ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Product information - product name
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Product information - product price
        /// </summary>
        public decimal ProductPrice { get; set; }
    }
}
