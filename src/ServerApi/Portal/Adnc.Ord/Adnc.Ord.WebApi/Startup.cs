using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using Autofac;
using Adnc.Infr.Consul;
using Adnc.Ord.Application;
using Adnc.WebApi.Shared;
using Adnc.Infr.EventBus;
using Adnc.Ord.Application.EventSubscribers;
using Adnc.Ord.Application.RpcServices;

namespace Adnc.Ord.WebApi
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
                registion.AddEventBusSubscribers(EbConsts.CapTableNamePrefix, EbConsts.CapDefaultGroup, (srv) =>
                {
                    srv.AddScoped<WarehouseQtyBlockedEventSubscriber>();
                });

                var whseServiceAddress = (_environment.IsProduction() || _environment.IsStaging()) ? "adnc.whse.webapi" : "http://localhost:8065";
                var policies = registion.GenerateDefaultRefitPolicies();
                registion.AddRpcService<IWhseRpcService>(whseServiceAddress, policies);
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAdncModules<AdncOrdApplicationModule>(_configuration);
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