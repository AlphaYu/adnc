using Adnc.Infra.Mapper;
using Adnc.Shared.Rpc.Grpc.Rtos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Adnc.Whse.WebApi.Grpc;

public class WhseGrpcServer : Adnc.Shared.Rpc.Grpc.Services.WhseGrpc.WhseGrpcBase
{
    private readonly IProductAppService _productAppService;
    private readonly IObjectMapper _mapper;

    public WhseGrpcServer(IProductAppService productAppService
        , IObjectMapper mapper)
    {
        _productAppService = productAppService;
        _mapper = mapper;
    }

    public override async Task<GrpcResponse> GetProducts(ProductSearchRequest request, ServerCallContext context)
    {
        var grpcResponse = new GrpcResponse();
        var searchDto = _mapper.Map<ProductSearchListDto>(request);
        var products = await _productAppService.GetListAsync(searchDto) ;
        if (products.IsNotNullOrEmpty())
        {
            var replyList = _mapper.Map<ProductListReply>(products);
            grpcResponse.Content = Any.Pack(replyList);
        }
        grpcResponse.IsSuccessStatusCode = true;
        return grpcResponse;
    }
}
