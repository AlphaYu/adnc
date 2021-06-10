using Adnc.Infra.Application.Dtos;

namespace Adnc.Ord.Application.Contracts.Dtos
{
    public class OrderUpdationDto : IDto
    {
        /// <summary>
        /// 收货信息
        /// </summary>
        public OrderReceiverDto DeliveryInfomaton { get; set; }
    }
}