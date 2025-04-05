using Adnc.Demo.Remote.Grpc.Messages;
using Adnc.Demo.Remote.Grpc.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Adnc.Demo.Admin.Api.Grpc;

public class AuthGrpcServer( IOptions<JWTOptions> jwtOptions, IUserService userService, IObjectMapper mapper) : AuthGrpc.AuthGrpcBase
{

    [AllowAnonymous]
    public override async Task<GrpcResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var loginDto = mapper.Map<UserLoginDto>(request);
        var loginResult = await userService.LoginAsync(loginDto);
        var grpcResponse = new GrpcResponse() { IsSuccessStatusCode = loginResult.IsSuccess };
        if (!grpcResponse.IsSuccessStatusCode)
        {
            grpcResponse.Error = loginResult.ProblemDetails?.Detail;
            return grpcResponse;
        }

        var validatedInfo = loginResult.Content;
        var loginReply = new LoginReply
        {
            Token = JwtTokenHelper.CreateAccessToken(jwtOptions.Value, validatedInfo.ValidationVersion, validatedInfo.Account, validatedInfo.Id.ToString(), validatedInfo.Name, validatedInfo.GetRoleIdsString(), BearerDefaults.Manager).Token,
            RefreshToken = JwtTokenHelper.CreateRefreshToken(jwtOptions.Value, validatedInfo.ValidationVersion, validatedInfo.Id.ToString()).Token
        };
        grpcResponse.Content = Any.Pack(loginReply);
        return grpcResponse;
    }
}
