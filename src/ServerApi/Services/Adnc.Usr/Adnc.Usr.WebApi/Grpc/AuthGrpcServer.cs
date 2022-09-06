using Adnc.Infra.Mapper;
using Adnc.Shared.Rpc.Grpc.Messages;
using Adnc.Shared.Rpc.Grpc.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Adnc.Usr.WebApi.Grpc;

public class AuthGrpcServer : AuthGrpc.AuthGrpcBase
{
    private readonly IOptions<JwtConfig> _jwtOptions;
    private readonly IAccountAppService _accountService;
    private readonly IObjectMapper _mapper;

    public AuthGrpcServer(
        IOptions<JwtConfig> jwtOptions
        , IAccountAppService accountService
        , IObjectMapper mapper)
    {
        _jwtOptions = jwtOptions;
        _accountService = accountService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    public override async Task<GrpcResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var loginDto = _mapper.Map<UserLoginDto>(request);
        var loginResult = await _accountService.LoginAsync(loginDto);
        var grpcResponse = new GrpcResponse() { IsSuccessStatusCode = loginResult.IsSuccess };
        if (!grpcResponse.IsSuccessStatusCode)
        {
            grpcResponse.Error = loginResult.ProblemDetails?.Detail;
            return grpcResponse;
        }

        var validatedInfo = loginResult.Content;
        var loginReply = new LoginReply
        {
            Token = JwtTokenHelper.CreateAccessToken(_jwtOptions.Value, validatedInfo.ValidationVersion, validatedInfo.Account, validatedInfo.Id.ToString(), validatedInfo.Name, validatedInfo.RoleIds, JwtBearerDefaults.Manager).Token,
            RefreshToken = JwtTokenHelper.CreateRefreshToken(_jwtOptions.Value, validatedInfo.ValidationVersion, validatedInfo.Id.ToString()).Token
        };
        grpcResponse.Content = Any.Pack(loginReply);
        return grpcResponse;
    }
}