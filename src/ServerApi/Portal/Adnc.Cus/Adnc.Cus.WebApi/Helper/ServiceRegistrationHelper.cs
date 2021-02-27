using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Adnc.WebApi.Shared;
using Adnc.Application.Shared.RpcServices;
using Adnc.Infr.Consul;
using Adnc.Cus.Application.EventSubscribers;
using Adnc.Infr.EventBus;

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
            _services.AddSingleton<ITokenGenerator, DefaultTokenGenerator>();

            var defaultPolicies = base.GenerateDefaultRefitPolicies();
            //Func<Task<string>> defaultGetToken = GetTokenDefaultFunc;

            //注册用户认证、鉴权服务Rpc服务到容器
            var authServerAddress = (_env.IsProduction() || _env.IsStaging()) ? "adnc.usr.webapi" : "http://localhost:5010";
            base.AddRpcService<IAuthRpcService>(authServerAddress, defaultPolicies);
            //注册运维RPC服务到容器
            var maintServiceAddress = (_env.IsProduction() || _env.IsStaging()) ? "adnc.maint.webapi" : "http://localhost:5020";
            base.AddRpcService<IMaintRpcService>(maintServiceAddress, defaultPolicies);
        }

        public void AddAllEventBusSubscribers(string tableNamePrefix = EbConsts.CapTableNamePrefix, string groupName = EbConsts.CapDefaultGroup)
        {
            base.AddEventBusSubscribers(tableNamePrefix, groupName,(s)=>
            {
                s.AddScoped<CustomerRechargedEventSubscriber>();
            });
        }
    }
}