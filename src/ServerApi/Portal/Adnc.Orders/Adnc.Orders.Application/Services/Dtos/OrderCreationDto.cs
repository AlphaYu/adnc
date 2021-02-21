using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Orders.Application.Dtos
{
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
        public OrderDeliveryInfomationDto DeliveryInfomaton { get; set; }

        /// <summary>
        /// 订单子项
        /// </summary>
        public virtual ICollection<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto: IDto
    {
        public string ProductId { get; set; }

        public int Count { get; set; }
    }
}
