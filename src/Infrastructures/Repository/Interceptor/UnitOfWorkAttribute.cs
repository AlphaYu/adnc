namespace Adnc.Infra.Repository.Interceptor;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class UnitOfWorkAttribute : Attribute
{
    /// <summary>
    /// 需要把事务共享给CAP
    /// </summary>
    public bool Distributed { get; set; }
}
