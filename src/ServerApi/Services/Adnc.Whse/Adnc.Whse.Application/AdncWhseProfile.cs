using Adnc.Application.Shared.Dtos;
using Adnc.Infra.IRepositories;
using Adnc.Whse.Application.Contracts.Dtos;
using Adnc.Whse.Domain.Entities;
using AutoMapper;

namespace Adnc.Whse.Application
{
    public class AdncWhseProfile : Profile
    {
        public AdncWhseProfile()
        {
            CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());

            CreateMap<Product, ProductDto>();

            CreateMap<Warehouse, WarehouseDto>();
        }
    }
}