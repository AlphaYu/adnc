using Adnc.Shared.RpcServices.Services;
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

namespace Adnc.Maint.WebApi
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
            services.AddAdncServices<PermissionHandlerRemote>();
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