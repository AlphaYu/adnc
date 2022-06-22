using Adnc.Gateway.Ocelot.Identity;
using Adnc.Infra.Core.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace Adnc.Gateway.Ocelot;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        var threadPoolConfig = Configuration.GetThreadPoolSettingsSection();
        services.Configure<ThreadPoolSettings>(threadPoolConfig);

        var authenticationProviderKey = "mgmt";
        var jwtConfig = Configuration.GetJWTSection().Get<JwtConfig>();
        services
            .AddAuthentication()
            .AddJwtBearer(authenticationProviderKey, options =>
            {
                options.TokenValidationParameters = JwtSecurityTokenHandlerExtension.GenarateTokenValidationParameters(jwtConfig);
            });

        services
            .AddCors(options =>
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
            })
            .AddOcelot(Configuration)
            .AddConsul()
            .AddPolly();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app
            .UseCors("default")
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Hello Ocelot,{env.EnvironmentName}!");
                });
            })
            .UseOcelot()
            .Wait();
    }
}
