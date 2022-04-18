namespace Adnc.Infra.Core
{
    public interface IServiceInfo
    {
        public string Id { get; set; }
        public string CorsPolicy { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyFullName { get; set; }
    }
}