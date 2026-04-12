using System.Runtime.Loader;

namespace Adnc.Shared.Application;

public sealed class DependencyAssemblyInfo
{
    private readonly Assembly _applicationAssembly;
    private readonly string _assemblyNamePrefix;

    public DependencyAssemblyInfo(Assembly applicationAssembly)
    {
        _applicationAssembly = applicationAssembly;
        var assemblyName = _applicationAssembly.GetName().Name ?? throw new InvalidOperationException("Application assembly name cannot be null");
        _assemblyNamePrefix = assemblyName.Remove(assemblyName.LastIndexOf('.'));
    }

    public Assembly ApplicationLayerAssembly
    {
        get
        {
            //var assemblyName = $"{_assemblyNamePrefix}.Application";
            //var assembly = GetAssemblyByName(assemblyName);
            //return assembly ?? throw new InvalidOperationException($"Cannot find assembly with name {assemblyName}");
            return _applicationAssembly;
        }
    }

    public Assembly ContractLayerAssembly
    {
        get
        {
            var assemblyName = $"{_assemblyNamePrefix}.Application.Contracts";
            var assembly = GetAssemblyByName(assemblyName);
            return assembly ?? throw new InvalidOperationException($"Cannot find assembly with name {assemblyName}");
        }
    }

    public Assembly RepositoryOrDomainLayerAssembly
    {
        get
        {
            var assemblyName = $"{_assemblyNamePrefix}.Repository";
            var assembly = GetAssemblyByName(assemblyName);
            if (assembly is null)
            {
                assemblyName = $"{_assemblyNamePrefix}.Domain";
                assembly = GetAssemblyByName(assemblyName);
            }
            return assembly ?? throw new InvalidOperationException($"Cannot find assembly with name {assemblyName}");
        }
    }

    private Assembly? GetAssemblyByName(string name)
    {
        var assembly = AssemblyLoadContext.Default.Assemblies.FirstOrDefault(a => a.GetName().Name == name);
        if (assembly is not null)
        {
            return assembly;
        }
        else
        {
            var referencedAssemblyName = _applicationAssembly.GetReferencedAssemblies().Where(x => x.Name == name).FirstOrDefault();
            if (referencedAssemblyName is not null)
            {
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(referencedAssemblyName);
            }
            else
            {
                var candidatePath = Path.Combine(AppContext.BaseDirectory, name + ".dll");
                if (File.Exists(candidatePath))
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(candidatePath);
                }
            }
            return assembly;
        }
    }
}
