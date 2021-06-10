using Adnc.Infra.Application.Dtos;
using Adnc.Whse.Application.Contracts.Dtos;
using AutoMapper;
using Adnc.Infra.IRepositories;
using Adnc.Whse.Domain.Entities;

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