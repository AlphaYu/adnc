using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace Adnc.Gateway.Ocelot;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ThreadPoolSettings>(Configuration.GetThreadPoolSettingsSection());

        services.AddCors(options =>
        {
            options.AddPolicy("default", policy =>
            {
                var corsHosts = Configuration.GetValue<string>("CorsHosts");
                var corsHostsArray = corsHosts.Split(',');
                policy.WithOrigins(corsHostsArray)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });

        //��ʹ��consul,��Ӧocelot.direct.json
        //services.AddOcelot();

        //ʹ��consul,��Ӧocelot.consul.json
        services.AddOcelot().AddConsul();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("default");
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync($"Hello Ocelot,{env.EnvironmentName}!");
            });
        });
        app.UseOcelot().Wait();
    }
}
