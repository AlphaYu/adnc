using Adnc.Application.Shared.Dtos;
using Adnc.Infra.IRepositories;
using Adnc.Ord.Application.Contracts.Dtos;
using Adnc.Ord.Domain.Entities;
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