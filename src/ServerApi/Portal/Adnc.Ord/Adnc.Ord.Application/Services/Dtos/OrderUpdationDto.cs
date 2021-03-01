using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Ord.Application.Dtos
{
    public class OrderUpdationDto : IDto
    {
        /// <summary>
        /// 收货信息
        /// </summary>
        public OrderReceiverDto DeliveryInfomaton { get; set; }
    }
}
