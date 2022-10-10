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
        var authenticationProviderKey = "mgmt";
        var threadPoolConfig = Configuration.GetSection("ThreadPoolSettings");

        services
            .Configure<ThreadPoolSettings>(threadPoolConfig)
            .AddAuthentication()
            .AddJwtBearer(authenticationProviderKey, options =>
            {
                var bearerConfig = Configuration.GetSection("JWT").Get<JWTOptions>();
                options.TokenValidationParameters = JwtSecurityTokenHandlerExtension.GenarateTokenValidationParameters(bearerConfig);
            })
            ;
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
            .AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            })
            .AddOcelot(Configuration)
            .AddConsul()
            .AddPolly()
            ;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app
            .UseCors("default")
            //.UseHttpLogging()
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
