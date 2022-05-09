﻿using Adnc.Infra.Mapper;
using Adnc.Shared.Application.Contracts;
using Adnc.Shared.Rpc.Grpc.Messages;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Adnc.Usr.WebApi.Grpc;

public class UsrGrpcServer : Adnc.Shared.Rpc.Grpc.Services.UsrGrpc.UsrGrpcBase
{
    private readonly IUserAppService _userService;
    private readonly IDeptAppService _deptAppService;
    private readonly UserContext _userContext;
    private readonly IObjectMapper _mapper;

    public UsrGrpcServer(IUserAppService userService
        , IDeptAppService deptAppService
        , UserContext userContext
        , IObjectMapper mapper)
    {
        _userService = userService;
        _deptAppService = deptAppService;
        _userContext = userContext;
        _mapper = mapper;
    }

    public override async Task<GrpcResponse> GetCurrenUserPermissions(UserPermissionsRequest request, ServerCallContext context)
    {
        var grpcResponse = new GrpcResponse() { IsSuccessStatusCode = true };
        if (request.UserId != _userContext.Id)
        {
            grpcResponse.IsSuccessStatusCode = false;
            grpcResponse.Error = @"request.UserId != _userContext.Id";
            return grpcResponse;
        }
        var result = await _userService.GetPermissionsAsync(_userContext.Id, request.Permissions, request.ValidationVersion);
        result ??= new List<string>();
        var response = new UserPermissionsReply();
        response.Permissions.AddRange(result);
        grpcResponse.Content = Any.Pack(response);
        return grpcResponse;
    }

    public override async Task<GrpcResponse> GetDepts(Empty request, ServerCallContext context)
    {
        var grpcResponse = new GrpcResponse() { IsSuccessStatusCode = true };
        var depts = await _deptAppService.GetTreeListAsync();
        var replyDepts = depts.IsNullOrEmpty()
                                    ? new List<DeptReply>()
                                    : _mapper.Map<List<DeptReply>>(depts);
        var replyList = new DeptListReply();
        replyList.List.AddRange(replyDepts);
        grpcResponse.Content = Any.Pack(replyList);
        return grpcResponse;
    }
}