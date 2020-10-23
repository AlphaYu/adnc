using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Adnc.WebApi.Shared
{
    public class ServiceInfo
    {
        public string CorsPolicy { get; set; } = "default";
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string AssemblyName { get; set; }

        public static ServiceInfo Create(Assembly assembly)
        {
            var description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version;
            var fullName = assemblyName.Name.ToLower();
            var startIndex = fullName.IndexOf('.') + 1;
            var endIndex = fullName.IndexOf('.', startIndex);
            var shortName = fullName.Substring(startIndex, endIndex - startIndex);

            return new ServiceInfo
            {
                FullName = fullName
                ,
                ShortName = shortName
                ,
                AssemblyName = assemblyName.Name
                ,
                Description = description
                ,
                Version = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision.ToString("00"))
            };
        }
    }
}
