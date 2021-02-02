using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Adnc.WebApi.Shared;
using Adnc.Cus.Core.EventBus.Subscribers;
using Adnc.Cus.Core;
using Adnc.Application.Shared.RpcServices;
using System;
using System.Threading.Tasks;

namespace Adnc.Cus.WebApi.Helper
{
    public sealed class ServiceRegistrationHelper : SharedServicesRegistration
    {
        public ServiceRegistrationHelper(IConfiguration cfg
            , IServiceCollection services
            , IWebHostEnvironment env
            , ServiceInfo serviceInfo)
          : base(cfg, services, env, serviceInfo)
        {
        }

        public void AddAllRpcServices()
        {
            var defaultPolicies = base.GenerateDefaultRefitPolicies();
            Func<Task<string>> defaultGetToken = GetTokenDefaultFunc;

            //注册用户认证、鉴权服务Rpc服务到容器
            var authServerAddress = (_env.IsProduction() || _env.IsStaging()) ? "adnc.usr.webapi" : "http://localhost:5010";
            base.AddRpcService<IAuthRpcService>(authServerAddress, defaultPolicies,defaultGetToken);
            //注册运维RPC服务到容器
            var maintServiceAddress = (_env.IsProduction() || _env.IsStaging()) ? "adnc.maint.webapi" : "http://localhost:5020";
            base.AddRpcService<IMaintRpcService>(maintServiceAddress, defaultPolicies,defaultGetToken);
        }

        public void AddAllEventBusSubscribers(string tableNamePrefix = "Cap", string groupName = EbConsts.CapDefaultGroup)
        {
            base.AddEventBusSubscribers(tableNamePrefix, groupName,(s)=>
            {
                s.AddScoped<ICustomerRechargedSubscriber, CustomerRechargedSubscriber>();
            });
        }
    }
}