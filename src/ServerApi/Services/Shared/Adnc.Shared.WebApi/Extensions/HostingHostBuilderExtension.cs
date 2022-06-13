using NLog;
using NLog.Web;

namespace Microsoft.Extensions.Hosting;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder ConfigureAdncDefault(this WebApplicationBuilder builder, string[] args, Assembly webApiAssembly)
    {
        if (builder is null) 
            throw new ArgumentNullException(nameof(builder));

        _serviceInfo = new ServiceInfo(webApiAssembly, builder.Environment);
        var initialData = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("ServiceName", _serviceInfo.ServiceName) };

        // Configuration
        builder.Services.ReplaceConfiguration(builder.Configuration);
        builder.Configuration.AddCommandLine(args);
        builder.Configuration.AddInMemoryCollection(initialData);
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.shared.development.json", true, true);
        }
        else if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
        {
            var consulOption = builder.Configuration.GetConsulSection().Get<ConsulConfig>();
            builder.Configuration.AddConsulConfiguration(consulOption, true);
        }
        OnSettingConfigurationChanged(builder.Configuration);

        //ServiceCollection
        builder.Services.AddSingleton(typeof(IServiceInfo), _serviceInfo);
        builder.Services.AddAdnc(webApiAssembly);

        //Logging
        builder.Logging.ClearProviders();
        var logContainer = builder.Configuration.GetValue("Logging:LogContainer", "console");
        LogManager.LoadConfiguration($"{AppContext.BaseDirectory}/NLog/nlog-{logContainer}.config");
        builder.Host.UseNLog();

        //WebHost(Kestrel)
        builder.WebHost.ConfigureKestrel((context, serverOptions) =>
        {
            var kestrelSection = context.Configuration.GetKestrelSection();
            serverOptions.Configure(kestrelSection);
        });

        return builder;
    }

    /// <summary>
    /// replace placeholder
    /// </summary>
    /// <param name="sections"></param>
    /// <param name="serviceInfo"></param>
    private static void ReplacePlaceholder(IEnumerable<IConfigurationSection> sections, IServiceInfo serviceInfo)
    {
        foreach (var section in sections)
        {
            var childrenSections = section.GetChildren();
            if (childrenSections.IsNotNullOrEmpty())
                ReplacePlaceholder(childrenSections, serviceInfo);

            if (section.Value.IsNullOrWhiteSpace())
                continue;

            var sectionValue = section.Value;
            if (sectionValue.Contains("$ServiceName"))
                section.Value = sectionValue.Replace("$ServiceName", serviceInfo.ServiceName);

            if (sectionValue.Contains("$ShortName"))
                section.Value = sectionValue.Replace("$ShortName", serviceInfo.ShortName);
        }
    }

    /// <summary>
    /// Register Cofiguration ChangeCallback
    /// </summary>
    /// <param name="state"></param>
    private static IDisposable _callbackRegistration;
    private static IServiceInfo _serviceInfo;
    private static void OnSettingConfigurationChanged(object state)
    {
        _callbackRegistration?.Dispose();
        var configuration = state as IConfiguration;
        var changedChildren = configuration.GetChildren();
        var reloadToken = configuration.GetReloadToken();

        ReplacePlaceholder(changedChildren, _serviceInfo);

        _callbackRegistration = reloadToken.RegisterChangeCallback(OnSettingConfigurationChanged, state);
    }
}