namespace Adnc.Infra.Core.Interfaces;

/// <summary>
/// Interface for serviceinfo.
/// </summary>
public interface IServiceInfo
{
    /// <summary>
    /// The ID associated with the service.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// The name associated with the service.
    /// </summary>
    public string ServiceName { get; }

    /// <summary>
    /// The cross-origin resource sharing (CORS) policy associated with the service.
    /// </summary>
    public string CorsPolicy { get; set; }

    /// <summary>
    /// The short name associated with the service.
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// The relative root path associated with the service.
    /// </summary>
    public string RelativeRootPath { get; }

    /// <summary>
    /// The version associated with the service.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// The description associated with the service.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// The assembly associated with the service startup.
    /// </summary>
    public Assembly StartAssembly { get; }

    /// <summary>
    /// Migrations Assembly Name
    /// </summary>
    public string MigrationsAssemblyName { get; }
}