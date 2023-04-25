using Adnc.Infra.Consul.Configuration;

namespace Adnc.Shared.WebApi
{
    public static class ServiceInfoExtension
    {
        private static readonly object lockObj = new();
        private static Assembly? appAssembly;

        /// <summary>
        /// 获取WebApiAssembly程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetWebApiAssembly(this IServiceInfo serviceInfo) => serviceInfo.StartAssembly;

        /// <summary>
        /// 获取Application程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetApplicationAssembly(this IServiceInfo serviceInfo)
        {
            if (appAssembly is null)
            {
                lock (lockObj)
                {
                    if (appAssembly is null)
                    {
                        Assembly startAssembly = serviceInfo.StartAssembly ?? throw new NullReferenceException(nameof(serviceInfo.StartAssembly));
                        string startAssemblyFullName = startAssembly.FullName ?? throw new NullReferenceException(nameof(startAssembly.FullName));
                        string startAssemblyName = startAssembly.GetName().Name ?? string.Empty;
                        string lastName = startAssemblyName.Split(".").Last();
                        string appAssemblyFullName = startAssemblyFullName.Replace($".{lastName}", ".Application");
                        appAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == appAssemblyFullName);
                        if (appAssembly is null)
                        {
                            appAssembly = startAssembly;
                            //appAssembly = Assembly.Load(appAssemblyName);
                            //var appAssemblyPath = serviceInfo.AssemblyLocation.Replace(".WebApi.dll", ".Application.dll");
                            ///appAssembly = Assembly.LoadFrom(appAssemblyPath);
                        }
                    }
                }
            }
            return appAssembly;
        }

        /// <summary>
        /// 获取导航首页内容
        /// </summary>
        /// <param name="serviceInfo"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public static string GetDefaultPageContent(this IServiceInfo serviceInfo, IServiceProvider serviceProvider)
        {
            var swaggerUrl = $"/{serviceInfo.RelativeRootPath}/index.html";
            var consulOptions = serviceProvider.GetRequiredService<IOptions<ConsulOptions>>();
            var healthCheckUrl = consulOptions?.Value?.HealthCheckUrl ?? $"/{serviceInfo.RelativeRootPath}/health-24b01005-a76a-4b3b-8fb1-5e0f2e9564fb";
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var content = $"<div align='center'><a href='https://github.com/alphayu/adnc' target='_blank'><img src='https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-topics.png'/></a><br>" +
                $"ASPNETCORE_ENVIRONMENT = {envName} <br> " +
                $"Version = {serviceInfo.Version} <br> " +
                $"ServiceName = {serviceInfo.ServiceName} <br> " +
                $"ShortName = {serviceInfo.ShortName} <br> " +
                $"RelativeRootPath = {serviceInfo.RelativeRootPath} <br> " +
                $"<br><a href='{swaggerUrl}'>swagger UI</a> | <a href='{healthCheckUrl}'>healthy checking</a><br>" +
                $"<br>{DateTime.Now}</div>";
            return content;
        }
    }
}