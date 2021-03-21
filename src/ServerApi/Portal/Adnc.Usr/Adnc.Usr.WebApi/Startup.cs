using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Autofac;
using Adnc.Infr.Common;
using Adnc.Infr.Consul;
using Adnc.Infr.Mongo;
using Adnc.Infr.EfCore;
using Adnc.Usr.Application;
using Adnc.Usr.WebApi.Helper;
using Adnc.WebApi.Shared;
using Adnc.WebApi.Shared.Middleware;

namespace Adnc.Usr.WebApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly ServiceInfo _serviceInfo;
        private ServiceRegistrationHelper _srvRegistration;

        public Startup(IConfiguration configuration
            , IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            _serviceInfo = ServiceInfo.Create(Assembly.GetExecutingAssembly());
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<UserContext>();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            _srvRegistration = new ServiceRegistrationHelper(Configuration, services, _env, _serviceInfo);
            _srvRegistration.Configure();
            _srvRegistration.AddControllers();
            _srvRegistration.AddJWTAuthentication();
            _srvRegistration.AddAuthorization<PermissionHandlerLocal>();
            _srvRegistration.AddCors();
            _srvRegistration.AddHealthChecks();
            _srvRegistration.AddEfCoreContext();
            _srvRegistration.AddMongoContext();
            _srvRegistration.AddCaching();
            _srvRegistration.AddSwaggerGen();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //注册依赖模块
            //通过配置文件(autofac)注册
            //var module = new ConfigurationModule(Configuration);
            //builder.RegisterModule(module);
            builder.RegisterModule<AdncInfrMongoModule>();
            builder.RegisterModule<AdncInfrEfCoreModule>();
            builder.RegisterModule<AdncUsrApplicationModule>();
            builder.RegisterModule(new AdncInfrConsulModule(_srvRegistration.GetConsulConfig().ConsulUrl));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                //开启验证异常显示
                //PII is hidden 异常处理
                IdentityModelEventSource.ShowPII = true;
            }
            app.UseCustomExceptionHandler();
            app.UseRealIp(x =>
            {
                x.HeaderKeys = new string[] { "X-Forwarded-For", "X-Real-IP" };
            });
            app.UseCors(_serviceInfo.CorsPolicy);
            app.UseSwagger(c =>
            {
                c.RouteTemplate = $"/{_serviceInfo.ShortName}/swagger/{{documentName}}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"/", Description = _serviceInfo.Description } };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{_serviceInfo.ShortName}/swagger/{_serviceInfo.Version}/swagger.json", $"{_serviceInfo.FullName}-{_serviceInfo.Version}");
                c.RoutePrefix = $"{_serviceInfo.ShortName}";
            });
            app.UseHealthChecks($"/{_srvRegistration.GetConsulConfig().HealthCheckUrl}", new HealthCheckOptions()
            {
                Predicate = _ => true,
                // 该响应输出是一个json，包含所有检查项的详细检查结果
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseSSOAuthentication(_srvRegistration.IsSSOAuthentication);
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
            if (env.IsProduction() || env.IsStaging())
            {
                //注册本服务到consul
                app.RegisterToConsul();
            }
        }
    }
}