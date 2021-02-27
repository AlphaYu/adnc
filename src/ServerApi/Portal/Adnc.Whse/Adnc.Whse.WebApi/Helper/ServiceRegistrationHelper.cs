using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Adnc.WebApi.Shared;
using Adnc.Application.Shared.RpcServices;
using Adnc.Whse.Application.EventSubscribers;

namespace Adnc.Whse.WebApi.Helper
{
    public sealed class ServiceRegistrationHelper : SharedServicesRegistration
    {
        public IServiceCollection Services;

        public ServiceRegistrationHelper(IConfiguration cfg
            , IServiceCollection services
            , IWebHostEnvironment env
            , ServiceInfo serviceInfo)
          : base(cfg, services, env, serviceInfo)
        {
            Services = services;
        }

        public void AddAllRpcServices()
        {
            var policies = base.GenerateDefaultRefitPolicies();

            //注册用户认证、鉴权服务Rpc服务到容器
            var authServerAddress = (_env.IsProduction() || _env.IsStaging()) ? "adnc.usr.webapi" : "http://localhost:5010";
            base.AddRpcService<IAuthRpcService>(authServerAddress, policies);
            //注册运维RPC服务到容器
            var maintServiceAddress = (_env.IsProduction() || _env.IsStaging()) ? "adnc.maint.webapi" : "http://localhost:5020";
            base.AddRpcService<IMaintRpcService>(maintServiceAddress, policies);
        }

        public void AddAllEventBusSubscribers(string tableNamePrefix = "Cap", string groupName = "adnc-cap")
        {
            base.AddEventBusSubscribers(tableNamePrefix, groupName, s =>
            {
                //s.AddSingleton<ShelfToProductAllocatedEventSubscirber>();
                //add others......
            });
        }
    }
}
