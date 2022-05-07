using Adnc.Shared.Rpc.Grpc;
using Adnc.Shared.Rpc.Grpc.Rtos;
using Adnc.Shared.Rpc.Grpc.Services;

namespace Microsoft.AspNetCore.Authorization;

public sealed class PermissionHandlerRemote : AbstractPermissionHandler
{
    private readonly UsrGrpc.UsrGrpcClient _usrGrpcClient;

    public PermissionHandlerRemote(UsrGrpc.UsrGrpcClient usrGrpcClient) => _usrGrpcClient = usrGrpcClient;

    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> codes, string validationVersion)
    {
        var grpcRequest = new UserPermissionsRequest
        {
            UserId = userId,
            ValidationVersion = validationVersion
        };
        grpcRequest.Permissions.AddRange(codes);

        var result = await _usrGrpcClient.GetCurrenUserPermissionsAsync(grpcRequest, GrpcClientConsts.BearerHeader);
        if (result.IsSuccessStatusCode && result.Content.Is(UserPermissionsReply.Descriptor))
        {
            var grpcReply = result.Content.Unpack<UserPermissionsReply>();
            if (grpcReply.Permissions.IsNotNullOrEmpty())
                return true;
        }
        return false;
    }
}