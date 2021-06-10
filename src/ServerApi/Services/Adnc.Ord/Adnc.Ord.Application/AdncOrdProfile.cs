using Adnc.Infra.Application.Dtos;
using Adnc.Ord.Application.Contracts.Dtos;
using AutoMapper;
using Adnc.Infra.IRepositories;
using Adnc.Ord.Domain.Entities;

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