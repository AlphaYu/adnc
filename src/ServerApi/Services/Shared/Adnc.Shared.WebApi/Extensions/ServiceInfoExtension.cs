namespace Adnc.Shared.WebApi
{
    public static class ServiceInfoExtension
    {
        private static object lockObj = new();
        private static Assembly webApiAssembly;
        private static Assembly appAssembly;

        /// <summary>
        /// 获取WebApiAssembly程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetWebApiAssembly(this IServiceInfo _)
        {
            if (webApiAssembly is null)
            {
                lock (lockObj)
                {
                    if (webApiAssembly is null)
                        webApiAssembly = Assembly.GetEntryAssembly();
                }
            }
            return webApiAssembly;
        }

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
                        var appAssemblyName = serviceInfo.AssemblyFullName.Replace("WebApi", "Application");
                        appAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.EqualsIgnoreCase(appAssemblyName));
                        if (appAssembly is null)
                            appAssembly = Assembly.Load(appAssemblyName);
                        //var appAssemblyPath = serviceInfo.AssemblyLocation.Replace(".WebApi.dll", ".Application.dll");
                        ///appAssembly = Assembly.LoadFrom(appAssemblyPath);
                    }
                }
            }
            return appAssembly;
        }
    }
}