using Adnc.Gateway.Ocelot.Identity;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace Adnc.Gateway.Ocelot;

public class Startup(IConfiguration configuration)
{
    private readonly string _corsPolicyName = "default";

    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        var authenticationProviderKey = "mgmt";
        var threadPoolConfig = Configuration.GetSection("ThreadPoolSettings");

        services
            .Configure<ThreadPoolSettings>(threadPoolConfig)
            .AddAuthentication()
            .AddJwtBearer(authenticationProviderKey, options =>
            {
                var jwtOptions = Configuration.GetSection("JWT").Get<JWTOptions>();
                if (jwtOptions is not null)
                {
                    options.TokenValidationParameters = JwtSecurityTokenHandlerExtension.GenarateTokenValidationParameters(jwtOptions);
                }
            });

        var corsHosts = Configuration.GetValue<string>("CorsHosts") ?? string.Empty;
        Action<CorsPolicyBuilder> corsPolicyAction = (corsPolicy) => corsPolicy.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        if (corsHosts == "*")
        {
            corsPolicyAction += (corsPolicy) => corsPolicy.SetIsOriginAllowed(_ => true);
        }
        else
        {
            corsPolicyAction += (corsPolicy) => corsPolicy.WithOrigins(corsHosts.Split(','));
        }

        services
            .AddCors(option => option.AddPolicy(_corsPolicyName, corsPolicyAction))
            .AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            })
            .AddOcelot(Configuration)
            .AddConsul<IpAddressConsulServiceBuilder>()
            .AddPolly();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app
            .UseStaticFiles()
            .UseCors(_corsPolicyName)
            //.UseHttpLogging()
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var content = app.GetDefaultPageContent();
                    context.Response.Headers?.TryAdd("Content-Type", "text/html");
                    await context.Response.WriteAsync(content);
                });
            })
            .UseOcelot()
            .Wait();
    }
}
