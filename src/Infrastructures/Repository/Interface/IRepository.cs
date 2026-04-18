namespace Adnc.Infra.Repository;

/// <summary>
/// Base repository interface
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> where TEntity : class
{ }

public interface IRepository
{ }
