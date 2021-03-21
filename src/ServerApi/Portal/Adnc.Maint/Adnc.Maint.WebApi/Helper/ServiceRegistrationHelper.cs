using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Adnc.WebApi.Shared;
using Adnc.Application.Shared.RpcServices;

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
