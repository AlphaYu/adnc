namespace Adnc.Infra.Core.Interfaces;

public interface IDependencyRegistrar
{
    public string Name { get; }
    public void AddAdnc();
}
