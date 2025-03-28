﻿using Adnc.Shared.WebApi.Authorization;
using Adnc.Shared.WebApi.Authorization.Handlers;

namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册授权组件
    /// PermissionHandlerRemote 跨服务授权
    /// PermissionHandlerLocal  本地授权,adnc.usr走本地授权，其他服务走Rpc授权
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    protected virtual void AddAuthorization<TAuthorizationHandler>() where TAuthorizationHandler : AbstractPermissionHandler
    {
#pragma warning disable ASP0025 // Use AddAuthorizationBuilder
        Services
            .AddScoped<IAuthorizationHandler, TAuthorizationHandler>()
            .AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizePolicy.Default, policy =>
                {
                    policy.Requirements.Add(new PermissionRequirement());
                });
            });
#pragma warning restore ASP0025 // Use AddAuthorizationBuilder
    }
}
