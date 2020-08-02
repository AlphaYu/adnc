namespace Adnc.Core.IRepositories
{
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</>
    public interface IRepository<TEntity>
               where TEntity : class
    {
    }
}
