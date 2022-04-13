namespace Adnc.Infra.Core.Interfaces;

public interface IServiceInfo
{
    public string Id { get; }
    public string CorsPolicy { get; set; }
    public string ShortName { get; }
    public string FullName { get; }
    public string Version { get; }
    public string Description { get; }
    public string AssemblyName { get; }
    public string AssemblyFullName { get; }
    public string AssemblyLocation { get; }
}