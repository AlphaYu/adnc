namespace Adnc.Infra.Core.Interfaces;

/// <summary>
/// Interface for dependency registration.
/// </summary>
public interface IDependencyRegistrar
{
    /// <summary>
    /// The name associated with the dependency registrar.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Adds the ADNC dependency.
    /// </summary>
    public void AddAdnc();
}