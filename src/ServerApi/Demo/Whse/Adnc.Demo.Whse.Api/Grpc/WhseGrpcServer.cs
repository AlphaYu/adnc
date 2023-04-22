using Adnc.Infra.Mapper;
using Adnc.Demo.Shared.Rpc.Grpc.Messages;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Adnc.Demo.Whse.Api.Grpc;

public class WhseGrpcServer : Adnc.Demo.Shared.Rpc.Grpc.Services.WhseGrpc.WhseGrpcBase
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
        var products = await _productAppService.GetListAsync(searchDto);

        var replyProducts = products.IsNullOrEmpty()
                                        ? new List<ProductReply>()
                                        : _mapper.Map<List<ProductReply>>(products);

        var replyList = new ProductListReply();
        replyList.List.AddRange(replyProducts);
        grpcResponse.Content = Any.Pack(replyList);
        grpcResponse.IsSuccessStatusCode = true;
        return grpcResponse;
    }
}