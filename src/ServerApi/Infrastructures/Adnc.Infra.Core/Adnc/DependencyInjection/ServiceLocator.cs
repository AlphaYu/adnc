namespace Adnc.Infra.Core.DependencyInjection;

public static class ServiceLocator
{
    private static IServiceProvider? _provider;

    static ServiceLocator()
    {
    }

    /// <summary>
    /// 只能获取Singleton/Transient，获取Scoped周期的对象会存与构造函数获取的不是相同对象
    /// </summary>
    public static IServiceProvider? Provider
    {
        get { return _provider; }
        set
        {
            if (_provider is not null)
                throw new InvalidOperationException(nameof(_provider));
            else
                _provider = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}