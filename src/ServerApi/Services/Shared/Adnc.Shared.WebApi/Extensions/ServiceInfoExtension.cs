namespace System.Reflection
{
    public static class ServiceInfoExtension
    {
        private static readonly object lockObj = new();
        private static Assembly appAssembly;

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
                        var appAssemblyName = serviceInfo.StartAssembly.FullName.Replace("WebApi", "Application");
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