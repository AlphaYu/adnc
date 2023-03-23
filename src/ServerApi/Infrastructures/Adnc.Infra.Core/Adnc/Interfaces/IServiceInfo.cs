namespace Adnc.Infra.Core.Interfaces;

public interface IServiceInfo
{
    /// <summary>
    /// adnc-xxx-webapi-188933
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// adnc-xxx-webapi
    /// </summary>
    public string ServiceName { get; }

    /// <summary>
    /// corsPolicy
    /// </summary>
    public string CorsPolicy { get; set; }

    /// <summary>
    ///  usr-webapi or maint-webapi or cus-webapi or xxx
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// API relative root path
    /// </summary>
    public string RelativeRootPath { get; }

    /// <summary>
    /// 0.9.2.xx
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// assembly  of start's project
    /// </summary>
    public Assembly StartAssembly { get; }
}