using Adnc.Infra.Consul.Configuration;
using NLog;
using NLog.Web;

namespace Microsoft.Extensions.Hosting;

public static class WebApplicationBuilderExtension
{
    /// <summary>
    /// Register Cofiguration ChangeCallback
    /// </summary>
    private static IDisposable? _callbackRegistration;

    /// <summary>
    /// Configure Configuration/ServiceCollection/Logging
    /// <param name="builder"></param>
    /// <param name="serviceInfo"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// </summary>
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder, IServiceInfo serviceInfo)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(serviceInfo, nameof(serviceInfo));

        // Configuration
        var initialData = new List<KeyValuePair<string, string?>> { new(nameof(serviceInfo.ServiceName), serviceInfo.ServiceName) };
        builder.Configuration.AddInMemoryCollection(initialData);

        var configurationType = builder.Configuration.GetValue<string>(NodeConsts.ConfigurationType) ?? ConfigurationTypeConsts.File;
        switch (configurationType)
        {
            case ConfigurationTypeConsts.File:
                builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.shared.{builder.Environment.EnvironmentName}.json", true, true);
                break;
            case ConfigurationTypeConsts.Consul:
                var consulOption = builder.Configuration.GetSection(NodeConsts.Consul).Get<ConsulOptions>();
                if (consulOption is null || consulOption.ConsulKeyPath.IsNullOrWhiteSpace())
                {
                    throw new NotImplementedException(NodeConsts.Consul);
                }
                else
                {
                    consulOption.ConsulKeyPath = consulOption.ConsulKeyPath.Replace("$SHORTNAME", serviceInfo.ShortName);
                    builder.Configuration.AddConsulConfiguration(consulOption, true);
                }
                break;
            case ConfigurationTypeConsts.Nacos:
                throw new NotImplementedException(nameof(ConfigurationTypeConsts.Nacos));
            case ConfigurationTypeConsts.Etcd:
                throw new NotImplementedException(nameof(ConfigurationTypeConsts.Etcd));
            default:
                throw new NotImplementedException(nameof(configurationType));
        }

        OnSettingConfigurationChanged(builder.Configuration);

        builder.Services.ReplaceConfiguration(builder.Configuration);

        //Logging
        builder.Logging.ClearProviders();
        var logContainer = builder.Configuration.GetValue<string>(NodeConsts.Logging_LogContainer) ?? "console";
        LogManager.Setup().LoadConfigurationFromFile($"{AppContext.BaseDirectory}/NLog/nlog-{logContainer}.config");
        builder.Host.UseNLog();

        return builder;
    }

    /// <summary>
    /// replace placeholder
    /// </summary>
    /// <param name="sections"></param>
    private static void ReplacePlaceholder(IEnumerable<IConfigurationSection> sections)
    {
        var serviceInfo = ServiceInfo.GetInstance();
        foreach (var section in sections)
        {
            var childrenSections = section.GetChildren();
            if (childrenSections.IsNotNullOrEmpty())
            {
                ReplacePlaceholder(childrenSections);
            }

            if (section.Value.IsNullOrWhiteSpace())
            {
                continue;
            }

            var sectionValue = section.Value;
            if (!string.IsNullOrWhiteSpace(sectionValue))
            {
                if (sectionValue.Contains("$SERVICENAME"))
                {
                    section.Value = sectionValue.Replace("$SERVICENAME", serviceInfo.ServiceName);
                }

                if (sectionValue.Contains("$SHORTNAME"))
                {
                    section.Value = sectionValue.Replace("$SHORTNAME", serviceInfo.ShortName);
                }

                if (sectionValue.Contains("$RELATIVEROOTPATH"))
                {
                    section.Value = sectionValue.Replace("$RELATIVEROOTPATH", serviceInfo.RelativeRootPath);
                }
            }
        }
    }

    private static void OnSettingConfigurationChanged(object? state)
    {
        _callbackRegistration?.Dispose();
        if (state is not IConfiguration configuration)
        {
            throw new ArgumentNullException(nameof(state));
        }

        var changedChildren = configuration.GetChildren();
        var reloadToken = configuration.GetReloadToken();

        ReplacePlaceholder(changedChildren);

        _callbackRegistration = reloadToken.RegisterChangeCallback(OnSettingConfigurationChanged, state);
    }
}
