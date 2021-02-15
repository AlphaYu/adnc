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
using AutoMapper;
using Adnc.Infr.Common;
using Adnc.Infr.Consul.Registration;
using Adnc.Maint.WebApi.Helper;
using Adnc.Maint.Application;
using Adnc.WebApi.Shared;
using Adnc.WebApi.Shared.Middleware;
using Adnc.Infr.Consul;

namespace Adnc.Maint.WebApi
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
        public IServiceCollection ServiceCollection { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            ServiceCollection = services;
            services.AddScoped<UserContext>();
            services.AddAutoMapper(typeof(AdncMaintProfile));
            services.AddHttpContextAccessor();

            _srvRegistration = new ServiceRegistrationHelper(Configuration, services, _env, _serviceInfo);
            _srvRegistration.Configure();
            _srvRegistration.AddControllers();
            _srvRegistration.AddJWTAuthentication();
            _srvRegistration.AddAuthorization<PermissionHandlerRemote>();
            _srvRegistration.AddCors();
            _srvRegistration.AddHealthChecks();
            _srvRegistration.AddEfCoreContext();
            _srvRegistration.AddMongoContext();
            _srvRegistration.AddCaching();
            _srvRegistration.AddSwaggerGen();
            _srvRegistration.AddAllMqServices();
            _srvRegistration.AddAllRpcServices();

            services.AddConsulServices(_srvRegistration.GetConsulConfig());
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //ע������ģ��
            builder.RegisterModule<Adnc.Infr.Mongo.AdncInfrMongoModule>();
            builder.RegisterModule<Adnc.Infr.EfCore.AdncInfrEfCoreModule>();
            builder.RegisterModule(new Adnc.Maint.Application.AdncMaintApplicationModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
            app.UseCustomExceptionHandler();
            if (env.IsDevelopment())
            {
                //������֤�쳣��ʾ
                //PII is hidden �쳣����
                IdentityModelEventSource.ShowPII = true;
            }
            app.UseRealIp(x =>
            {
                //new string[] { "X-Real-IP", "X-Forwarded-For" }
                x.HeaderKeys = new string[] { "X-Forwarded-For", "X-Real-IP" };
            });
            app.UseCors();
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
                // ����Ӧ�����һ��json���������м�������ϸ�����
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
                //ע�᱾����consul
                app.RegisterToConsul();
            }
        }
    }
}