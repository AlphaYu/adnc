using NLog;
using NLog.Web;

namespace Microsoft.Extensions.Hosting;

public static class HostingHostBuilderExtension
{
    public static WebApplicationBuilder ConfigureAdncDefault(this WebApplicationBuilder builder, string[] args, Assembly webApiAssembly)
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));

        // Configuration
        builder.Services.ReplaceConfiguration(builder.Configuration);
        builder.Configuration.AddCommandLine(args);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.shared.development.json", true, true);
        }

        if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
        {
            var consulOption = builder.Configuration.GetConsulSection().Get<ConsulConfig>();
            builder.Configuration.AddConsulConfiguration(consulOption, true);
        }

        //ServiceCollection
        builder.Services.AddAdnc(webApiAssembly);

        //Logging
        builder.Logging.ClearProviders().AddConsole();
        //loki
        LogManager.LoadConfiguration($"{AppContext.BaseDirectory}/NLog/nlog-loki.config");
        //mongodb
        //LogManager.LoadConfiguration($"{AppContext.BaseDirectory}/NLog/nlog-mongodb.config");
        builder.Host.UseNLog();

        //WebHost(Kestrel)
        builder.WebHost.ConfigureKestrel((context, serverOptions) => serverOptions.Configure(context.Configuration.GetKestrelSection()));

        return builder;
    }
}