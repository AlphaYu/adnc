namespace Adnc.Shared.WebApi
{
    public static class ServiceInfoExtension
    {
        private static object lockObj = new();
        private static Assembly appAssembly;

        /// <summary>
        /// 获取Application程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetApplicationAssembly(this IServiceInfo serviceInfo)
        {
            if (appAssembly == null)
            {
                lock (lockObj)
                {
                    if (appAssembly == null)
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