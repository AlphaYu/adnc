using Adnc.Application.RpcService.Services;
using Adnc.Cus.Application.EventSubscribers;
using Adnc.Infra.Consul;
using Adnc.WebApi.Shared;
using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Adnc.Cus.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ServiceInfo _serviceInfo;

        public Startup(IConfiguration configuration
            , IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            _serviceInfo = ServiceInfo.Create(Assembly.GetExecutingAssembly());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAdncServices<PermissionHandlerRemote>(_configuration, _environment, _serviceInfo, (registion) =>
            {
                var policies = registion.GenerateDefaultRefitPolicies();
                var authServeiceAddress = _environment.IsDevelopment() ? "http://localhost:5010" : "adnc.usr.webapi";
                registion.AddRpcService<IAuthRpcService>(authServeiceAddress, policies);

                var maintServiceAddress = _environment.IsDevelopment() ? "http://localhost:5020" : "adnc.maint.webapi";
                registion.AddRpcService<IMaintRpcService>(maintServiceAddress, policies);

                registion.AddEventBusSubscribers<CapEventSubscriber>();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAdncModules(_configuration, _serviceInfo);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAdncMiddlewares();

            if (_environment.IsProduction() || _environment.IsStaging())
            {
                app.RegisterToConsul();
            }
        }
    }
}