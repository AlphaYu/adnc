using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Autofac;
using Autofac.Configuration;
using AutoMapper;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using HealthChecks.UI.Client;
using Adnc.Application;
using Adnc.Common;
using Adnc.Common.Helper;
using Adnc.Common.Models;
using Adnc.WebApi.Infrastructures.Middleware;
using Adnc.WebApi.Helper;
using Adnc.Infr.Consul;
using Adnc.Infr.Consul.Registration;

namespace Adnc.WebApi
{
    public class Startup
    {
        private readonly string _corsPolicy = "default";
        private readonly string _serviceName = "sys";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigurationHelper.Initialize(configuration);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var srvRegistration = ServiceRegistrationHelper.GetInstance(Configuration, services);
            srvRegistration.AddCaching();
            srvRegistration.AddEfCoreContext();
            srvRegistration.AddMongoContext();
            srvRegistration.AddControllers();
            srvRegistration.AddJWTAuthentication();
            srvRegistration.AddAuthorization();
            srvRegistration.AddCors(_corsPolicy);
            srvRegistration.AddSwaggerGen();
            srvRegistration.AddHealthChecks();
            services.AddAutoMapper(typeof(AdncProfile));
            services.AddScoped<UserContext>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var module = new ConfigurationModule(Configuration);
            builder.RegisterModule(module);
            builder.ConfigureCastleInterceptor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ContainerContext.Initialize(new IocContainer(app.ApplicationServices));

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
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders();
            app.UseCors(_corsPolicy);
            app.UseSwagger(c =>
            {
                c.RouteTemplate = $"/{_serviceName}/swagger/{{documentName}}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"/", Description ="系统管理相关Api"} };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{_serviceName}/swagger/v0.5.0/swagger.json", "Andc v0.5.0");
                c.RoutePrefix = $"{_serviceName}";
            });
            //app.UseErrorHandling();
            app.UseHealthChecks($"/{_serviceName}/health-24b01005-a76a-4b3b-8fb1-5e0f2e9564fb", new HealthCheckOptions()
            {
                Predicate = _ => true,
                // 该响应输出是一个json，包含所有检查项的详细检查结果
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCustomAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });

            //if (env.IsProduction() || env.IsStaging())
            {
                app.RegisterToConsul(Configuration.GetSection("Consul").Get<ConsulOption>());
            }
        }
    }
}