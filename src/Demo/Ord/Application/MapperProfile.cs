using Adnc.Demo.Ord.Application.Contracts.Dtos.Order;
using AutoMapper;

namespace Adnc.Demo.Ord.Application;

public class OrdProfile : Profile
{
    public OrdProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());

        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderDto.OrderItemDto>();
    }
}
