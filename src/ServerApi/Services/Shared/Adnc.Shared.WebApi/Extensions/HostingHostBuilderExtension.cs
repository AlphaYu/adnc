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
            builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.shared.development.json", true, true);

        if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
        {
            var consulOption = builder.Configuration.GetConsulSection().Get<ConsulConfig>();
            builder.Configuration.AddConsulConfiguration(consulOption, true);
        }

        var children = builder.Configuration.GetChildren();
        var serviceInfo = new ServiceInfo(webApiAssembly, builder.Environment);
        SetVariableValue(children, serviceInfo);

        //ServiceCollection
        builder.Services.AddSingleton(typeof(IServiceInfo), serviceInfo);
        builder.Services.AddAdnc(webApiAssembly);

        //Logging
        builder.Logging.ClearProviders().AddConsole();
        var logContainer = builder.Configuration.GetValue("Logging:LogContainer", string.Empty);
        if (logContainer.IsNotNullOrWhiteSpace())
            LogManager.LoadConfiguration($"{AppContext.BaseDirectory}/NLog/nlog-{logContainer}.config");
        builder.Host.UseNLog();

        //WebHost(Kestrel)
        builder.WebHost.ConfigureKestrel((context, serverOptions) => serverOptions.Configure(context.Configuration.GetKestrelSection()));

        return builder;
    }

    private static void SetVariableValue(IEnumerable<IConfigurationSection> sections, IServiceInfo serviceInfo)
    {
        sections.ForEach(section =>
        {
            var childrenSections = section.GetChildren();
            if (childrenSections.IsNotNullOrEmpty())
                SetVariableValue(childrenSections, serviceInfo);

            if (section.Value.IsNullOrWhiteSpace())
                return;

            if (section.Value.Equals("$ServiceName"))
                section.Value = serviceInfo.ServiceName;
            else if (section.Value.Equals("$ShortName"))
                section.Value = serviceInfo.ShortName;
        });
    }
}