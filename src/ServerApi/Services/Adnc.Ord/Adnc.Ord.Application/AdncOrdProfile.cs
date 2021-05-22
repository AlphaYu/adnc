using Adnc.Application.Shared.Dtos;
using Adnc.Core.Shared;
using Adnc.Ord.Application.Contracts.Dtos;
using Adnc.Ord.Core.Entities;
using AutoMapper;

namespace Adnc.Ord.Application
{
    public class AdncOrdProfile : Profile
    {
        public AdncOrdProfile()
        {
            CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());

            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderDto.OrderItemDto>();
        }
    }
}