using Adnc.Infra.Consul.Configuration;
using Adnc.Shared.WebApi.Middleware;

namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar : IMiddlewareRegistrar
{
    protected IApplicationBuilder App { get; init; } = default!;
    protected AbstractWebApiDependencyRegistrar(IApplicationBuilder app)
    {
        App = app;
    }

    /// <summary>
    /// 注册中间件入口方法
    /// </summary>
    /// <param name="app"></param>
    public abstract void UseAdnc();

    /// <summary>
    /// 注册webapi通用中间件
    /// </summary>
    protected virtual void UseWebApiDefault(
        Action<IApplicationBuilder>? beforeAuthentication = null,
        Action<IApplicationBuilder>? afterAuthentication = null,
        Action<IApplicationBuilder>? afterAuthorization = null,
        Action<IEndpointRouteBuilder>? endpointRoute = null)
    {
        ServiceLocator.Provider = App.ApplicationServices;
        var environment = App.ApplicationServices.GetRequiredService<IHostEnvironment>();
        var serviceInfo = App.ApplicationServices.GetRequiredService<IServiceInfo>();
        var consulOptions = App.ApplicationServices.GetRequiredService<IOptions<ConsulOptions>>();
        var configuration = App.ApplicationServices.GetRequiredService<IConfiguration>();
        var healthCheckUrl = consulOptions?.Value?.HealthCheckUrl ?? $"{serviceInfo.RelativeRootPath}/health-24b01005-a76a-4b3b-8fb1-5e0f2e9564fb";

        //var defaultFilesOptions = new DefaultFilesOptions();
        //defaultFilesOptions.DefaultFileNames.Clear();
        //defaultFilesOptions.DefaultFileNames.Add("index.html");
        //App
        //    .UseDefaultFiles(defaultFilesOptions)
        //    .UseStaticFiles();
        App
            .UseStaticFiles()
            .UseRealIp(x => x.HeaderKey = "X-Forwarded-For")
            .UseCustomExceptionHandler()
            .UseCors(serviceInfo.CorsPolicy);

        if (environment.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
        }

        var enableSwaggerUI = configuration.GetValue(NodeConsts.SwaggerUI_Enable, true);
        if(enableSwaggerUI)
        {
#if DEBUG
            App.UseMiniProfiler();
#endif
            App
                .UseSwagger(c =>
                {
                    c.RouteTemplate = $"/{serviceInfo.RelativeRootPath}/swagger/{{documentName}}/swagger.json";
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"/", Description = serviceInfo.Description } };
                    });
                })
                .UseSwaggerUI(c =>
                {
#if DEBUG
                    var assembly = serviceInfo.GetWebApiAssembly();
                    c.IndexStream = () =>
                    {
                        //var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.swagger_miniprofiler.html");
                        //return stream;
                        var miniProfiler = $"{AppContext.BaseDirectory}swagger_miniprofiler.html";
                        var text = File.ReadAllText(miniProfiler).Replace("$RELATIVEROOTPATH", serviceInfo.RelativeRootPath);
                        var byteArray = Encoding.UTF8.GetBytes(text);
                        var stream = new MemoryStream(byteArray);
                        return stream;
                    };
#endif
                    c.SwaggerEndpoint($"/{serviceInfo.RelativeRootPath}/swagger/{serviceInfo.Version}/swagger.json", $"{serviceInfo.ServiceName}-{serviceInfo.Version}");
                    c.RoutePrefix = $"{serviceInfo.RelativeRootPath}";
                });
        }
        App
            .UseHealthChecks($"/{healthCheckUrl}", new HealthCheckOptions()
            {
                Predicate = _ => true,
                // 该响应输出是一个json，包含所有检查项的详细检查结果
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            })
            .UseRouting();

        var enableMetrics = configuration.GetValue(NodeConsts.Metrics_Enable, true);
        if (enableMetrics)
        {
            App
                .UseHttpMetrics();

            DotNetRuntimeStatsBuilder
            .Customize()
            .WithContentionStats()
            .WithGcStats()
            .WithThreadPoolStats()
            .StartCollecting();
        }

        beforeAuthentication?.Invoke(App);
        App.UseAuthentication();
        afterAuthentication?.Invoke(App);
        App.UseAuthorization();
        afterAuthorization?.Invoke(App);

        App
            .UseEndpoints(endpoints =>
            {
                endpointRoute?.Invoke(endpoints);
                if(enableMetrics)
                {
                    endpoints.MapMetrics();
                }
                endpoints.MapControllers().RequireAuthorization();
            });
    }
}