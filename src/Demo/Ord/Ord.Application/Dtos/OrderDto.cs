namespace Adnc.Demo.Ord.Application.Dtos;

/// <summary>
/// 订单输出Dto
/// </summary>
public class OrderDto : OutputBaseAuditDto
{
    /// <summary>
    /// 客户Id
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 订单状态-状态码
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// 订单状态-状态名称
    /// </summary>
    public string StatusName { get; set; }

    /// <summary>
    /// 订单状态-状态变动原因
    /// </summary>
    public string StatusChangesReason { get; set; }

    /// <summary>
    /// 收货信息-收货人
    /// </summary>
    public string ReceiverName { get; set; }

    /// <summary>
    /// 收货信息-收货电话
    /// </summary>
    public string ReceiverPhone { get; set; }

    /// <summary>
    /// 收货信息-收货地址
    /// </summary>
    public string ReceiverAddress { get; set; }

    /// <summary>
    /// 订单子项
    /// </summary>
    public virtual ICollection<OrderItemDto> Items { get; set; }

    /// <summary>
    /// 订单子项Dto
    /// </summary>
    public class OrderItemDto : OutputDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 商品信息-商品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 商品信息-商品名
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品信息-商品价格
        /// </summary>
        public decimal ProductPrice { get; set; }
    }
}