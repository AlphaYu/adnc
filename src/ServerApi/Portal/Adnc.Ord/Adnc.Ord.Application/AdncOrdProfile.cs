using AutoMapper;
using Adnc.Ord.Application.Dtos;
using Adnc.Application.Shared.Dtos;
using Adnc.Ord.Domain.Entities;
using Adnc.Core.Shared;

namespace Adnc.Ord.Application
{
    public class AdncOrdProfile : Profile
    {
        public AdncOrdProfile()
        {
            CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());

            CreateMap<Order, OrderDto>();
                //.ForMember(dest => dest.ChangeStatusReason, opt => opt.MapFrom(src => src.Status.ChangeStatusReason))
                //.ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status.StatusCode))
                //.ForMember(dest => dest.DeliveryName, opt => opt.MapFrom(src => src.DeliveryInfomaton.Name))
                //.ForMember(dest => dest.DeliveryPhone, opt => opt.MapFrom(src => src.DeliveryInfomaton.Phone))
                //.ForMember(dest => dest.DeliveryAddress, opt => opt.MapFrom(src => src.DeliveryInfomaton.Address));
            CreateMap<OrderItem, OrderDto.OrderItemDto>();

        }
    }
}
