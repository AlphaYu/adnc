using Adnc.Demo.Whse.Application.Contracts.Dtos.Product;
using Adnc.Demo.Whse.Application.Contracts.Dtos.Warehouse;
using AutoMapper;

namespace Adnc.Demo.Whse.Application;

public class WhseProfile : Profile
{
    public WhseProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<Product, ProductDto>();
        CreateMap<Warehouse, WarehouseDto>();
    }
}
