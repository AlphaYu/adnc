using Adnc.Shared.Rpc.Grpc.Messages;

namespace Adnc.Whse.Application;

public class WhseProfile : Profile
{
    public WhseProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<Product, ProductDto>();
        CreateMap<Warehouse, WarehouseDto>();

        CreateMap<ProductSearchRequest, ProductSearchListDto>();
        CreateMap<ProductDto, ProductReply>();
    }
}