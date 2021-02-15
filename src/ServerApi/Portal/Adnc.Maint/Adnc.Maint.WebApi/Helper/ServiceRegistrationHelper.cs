using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Adnc.Maint.Application.Mq;
using Adnc.WebApi.Shared;
using Adnc.Application.Shared.RpcServices;
using Adnc.Infr.Consul.Consumer;
using Adnc.Infr.Common.Helper;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Threading.Tasks;
using Adnc.Infr.Consul;
using Consul;

namespace Adnc.Maint.WebApi.Helper
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

        /// <summary>
        /// 注册MQ消费者
        /// </summary>
        public void AddAllMqServices()
        {
            _services.AddHostedService<LoginLogMqConsumer>();
            _services.AddHostedService<OpsLogMqConsumer>();
        }

        /// <summary>
        /// 注册Rpc调用服务
        /// </summary>
        public void AddAllRpcServices()
        {
            var defaultPolicies = base.GenerateDefaultRefitPolicies();

            //注册用户认证、鉴权服务Rpc服务到容器
            var authServerAddress = (_env.IsProduction() || _env.IsStaging()) ? "adnc.usr.webapi" : "http://localhost:5010";
            base.AddRpcService<IAuthRpcService>(authServerAddress, defaultPolicies);
        }
    }
}
