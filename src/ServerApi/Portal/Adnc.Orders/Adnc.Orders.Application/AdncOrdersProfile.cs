using AutoMapper;
using Adnc.Orders.Application.Dtos;
using Adnc.Application.Shared.Dtos;
using Adnc.Orders.Domain.Entities;
using Adnc.Core.Shared;

namespace Adnc.Orders.Application
{
    public class AdncOrdersProfile : Profile
    {
        public AdncOrdersProfile()
        {
            CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
            
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderDto.OrderItemDto>();

        }
    }
}
