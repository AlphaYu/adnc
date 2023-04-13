using Adnc.Infra.Mapper;
using Adnc.Demo.Shared.Rpc.Grpc.Messages;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Adnc.Demo.Maint.Api.Grpc;

public class MaintGrpcServer : Adnc.Demo.Shared.Rpc.Grpc.Services.MaintGrpc.MaintGrpcBase
{
    private readonly IDictAppService _dictAppService;
    private readonly IObjectMapper _mapper;

    public MaintGrpcServer(IDictAppService dictAppService
        , IObjectMapper mapper)
    {
        _dictAppService = dictAppService;
        _mapper = mapper;
    }

    public override async Task<GrpcResponse> GetDict(DictRequest request, ServerCallContext context)
    {
        var grpcResponse = new GrpcResponse();
        var dict = await _dictAppService.GetAsync(request.Id);
        if (dict is not null)
        {
            var replyDict = _mapper.Map<DictReply>(dict);
            grpcResponse.Content = Any.Pack(replyDict);
        }
        grpcResponse.IsSuccessStatusCode = true;
        return grpcResponse;
    }
}