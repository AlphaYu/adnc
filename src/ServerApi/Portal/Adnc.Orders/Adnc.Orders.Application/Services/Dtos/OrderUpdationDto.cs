using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Orders.Application.Dtos
{
    public class OrderUpdationDto : IDto
    {
        /// <summary>
        /// 收货信息
        /// </summary>
        public OrderDeliveryInfomationDto DeliveryInfomaton { get; set; }
    }
}
