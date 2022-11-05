namespace Adnc.Infra.IRepositories;

/// <summary>
/// 仓储基类接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> where TEntity : class
{
}

public interface IRepository
{
}