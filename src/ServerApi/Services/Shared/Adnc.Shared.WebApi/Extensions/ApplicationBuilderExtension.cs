using Adnc.Shared.WebApi.Middleware;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtension
{
    /// <summary>
    /// 统一注册Adnc.WebApi通用中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configuration"></param>
    /// <param name="serviceInfo"></param>
    /// <param name="completedExecute"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAdncDefault(this IApplicationBuilder app
        , Action<IApplicationBuilder> beforeAuthentication = null
        , Action<IApplicationBuilder> afterAuthorization = null
        , Action<IEndpointRouteBuilder> endpointRoute = null)
    {
        ServiceLocator.Provider = app.ApplicationServices;
        var configuration = app.ApplicationServices.GetService<IConfiguration>();
        var environment = app.ApplicationServices.GetService<IHostEnvironment>();
        var serviceInfo = app.ApplicationServices.GetService<IServiceInfo>();
        var consulOptions = app.ApplicationServices.GetService<IOptions<ConsulConfig>>();       

        var defaultFilesOptions = new DefaultFilesOptions();
        defaultFilesOptions.DefaultFileNames.Clear();
        defaultFilesOptions.DefaultFileNames.Add("index.html");
        app
            .UseDefaultFiles(defaultFilesOptions)
            .UseStaticFiles()
            .UseCustomExceptionHandler()
            .UseRealIp(x => x.HeaderKey = "X-Forwarded-For")
            .UseCors(serviceInfo.CorsPolicy);

        if(environment.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
            app.UseMiniProfiler();
        }   

        app
            .UseSwagger(c =>
            {
                c.RouteTemplate = $"/{serviceInfo.ShortName}/swagger/{{documentName}}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"/", Description = serviceInfo.Description } };
                });
            })
            .UseSwaggerUI(c =>
            {
                var assembly = serviceInfo.GetWebApiAssembly();
                c.IndexStream = () => assembly.GetManifestResourceStream($"{assembly.GetName().Name}.swagger_miniprofiler.html");
                c.SwaggerEndpoint($"/{serviceInfo.ShortName}/swagger/{serviceInfo.Version}/swagger.json", $"{serviceInfo.ServiceName}-{serviceInfo.Version}");
                c.RoutePrefix = $"{serviceInfo.ShortName}";
            })
            .UseHealthChecks($"/{consulOptions.Value.HealthCheckUrl}", new HealthCheckOptions()
            {
                Predicate = _ => true,
                // 该响应输出是一个json，包含所有检查项的详细检查结果
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            })
            .UseRouting()
            .UseHttpMetrics();
            DotNetRuntimeStatsBuilder
            .Customize()
            .WithContentionStats()
            .WithGcStats()
            .WithThreadPoolStats()
            .StartCollecting();

        beforeAuthentication?.Invoke(app);
        app
            .UseAuthentication()
            .UseAuthorization();
        afterAuthorization?.Invoke(app);

        app
            .UseEndpoints(endpoints =>
            {
                endpointRoute?.Invoke(endpoints);
                endpoints.MapMetrics();
                endpoints.MapControllers().RequireAuthorization();
            });

        return app;
    }
}