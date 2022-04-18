using Adnc.Shared.RpcServices.Services;
using Adnc.Whse.Application.EventSubscribers;
using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Adnc.Whse.WebApi
{
    public class Startup
    {
        private readonly IHostEnvironment _environment;
        private IServiceCollection _services;

        public Startup(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;

            services.AddAdncServices<PermissionHandlerRemote>(registion =>
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
            builder.RegisterAdncModules(_services);
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