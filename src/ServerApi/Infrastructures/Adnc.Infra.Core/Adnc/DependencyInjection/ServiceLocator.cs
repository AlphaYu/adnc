namespace Adnc.Infra.Core.DependencyInjection;

public sealed class ServiceLocator
{
    private ServiceLocator()
    {
    }

    static ServiceLocator()
    {
    }

    /// <summary>
    /// 只能获取Singleton/Transient，获取Scoped周期的对象会存与构造函数获取的不是相同对象
    /// </summary>
    public static IServiceProvider? Provider { get; set; }
}