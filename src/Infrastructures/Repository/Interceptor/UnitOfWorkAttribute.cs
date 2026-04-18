namespace Adnc.Infra.Repository.Interceptor;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class UnitOfWorkAttribute : Attribute
{
    /// <summary>
    /// The transaction needs to be shared with CAP.
    /// </summary>
    public bool Distributed { get; set; }
}
