using AutoMapper;
using Adnc.Warehouse.Application.Dtos;
using Adnc.Application.Shared.Dtos;
using Adnc.Warehouse.Domain.Entities;
using Adnc.Core.Shared;

namespace Adnc.Warehouse.Application
{
    public class AdncWarehouseProfile : Profile
    {
        public AdncWarehouseProfile()
        {
            CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
            
            CreateMap<Product, ProductDto>();
            CreateMap<ProductStatus, ProductStatusDto>();

            CreateMap<Shelf, ShelfDto>();
            CreateMap<ShelfPosition, ShelfPositionDto>();
        }
    }
}
