using Adnc.Application.Shared.Dtos;
using Adnc.Core.Shared;
using Adnc.Whse.Application.Contracts.Dtos;
using Adnc.Whse.Core.Entities;
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