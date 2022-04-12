namespace Adnc.Whse.Application;

public class AdncWhseProfile : Profile
{
    public AdncWhseProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());

        CreateMap<Product, ProductDto>();

        CreateMap<Warehouse, WarehouseDto>();
    }
}