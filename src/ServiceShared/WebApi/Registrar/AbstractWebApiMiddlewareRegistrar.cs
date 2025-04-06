using Adnc.Infra.Consul.Configuration;
using Adnc.Shared.WebApi.Middleware;

namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiMiddlewareRegistrar(WebApplication app)
{
    protected WebApplication App { get; init; } = app;

    /// <summary>
    /// 注册中间件
    /// </summary>
    public abstract void UseAdnc();

    /// <summary>
    /// 注册webapi通用中间件
    /// </summary>
    protected void UseWebApiDefault(
        Action<WebApplication>? beforeAuthentication = null,
        Action<WebApplication>? afterAuthentication = null,
        Action<WebApplication>? afterAuthorization = null,
        Action<IEndpointRouteBuilder>? endpointRoute = null)
    {
        ServiceLocator.SetProvider(App.Services);
        var environment = App.Services.GetRequiredService<IHostEnvironment>();
        var serviceInfo = App.Services.GetRequiredService<IServiceInfo>();
        var consulOptions = App.Services.GetRequiredService<IOptions<ConsulOptions>>();
        var configuration = App.Services.GetRequiredService<IConfiguration>();
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
        if (enableSwaggerUI)
        {
            var relativeRootPath = serviceInfo.RelativeRootPath;
            var description = serviceInfo.Description;
            var serverName = serviceInfo.ServiceName;
            var version = serviceInfo.Version;
#if DEBUG
            App.UseMiniProfiler();
#endif
            App
                .UseSwagger(c =>
                {
                    c.RouteTemplate = $"/{relativeRootPath}/swagger/{{documentName}}/swagger.json";
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = [new() { Url = $"/", Description = description }];
                    });
                })
                .UseSwaggerUI(c =>
                {
#if DEBUG
                    var assembly = serviceInfo.StartAssembly;
                    c.IndexStream = () =>
                    {
                        //var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.swagger_miniprofiler.html");
                        //return stream;
                        var miniProfiler = $"{AppContext.BaseDirectory}swagger_miniprofiler.html";
                        var text = File.ReadAllText(miniProfiler).Replace("$RELATIVEROOTPATH", relativeRootPath);
                        var byteArray = Encoding.UTF8.GetBytes(text);
                        var stream = new MemoryStream(byteArray);
                        return stream;
                    };
#endif
                    c.SwaggerEndpoint($"/{relativeRootPath}/swagger/{version}/swagger.json", $"{serverName}-{version}");
                    c.RoutePrefix = $"{relativeRootPath}";
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

        var enableMetrics = configuration.GetValue(NodeConsts.Metrics_Enable, false);
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

        App.MapControllers().RequireAuthorization();
        if (enableMetrics)
        {
            App.MapMetrics();
        }
        endpointRoute?.Invoke(App);
    }
}
