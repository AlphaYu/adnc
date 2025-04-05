namespace Adnc.Infra.Repository;

public interface IAdoRepository : IRepository
{
    void ChangeOrSetDbConnection(IDbConnection dbConnection);

    [Obsolete($"use {nameof(CreateDbConnection)} instead")]
    IDbConnection ChangeOrSetDbConnection(string connectionString, DbTypes dbType);

    IDbConnection CreateDbConnection(string connectionString, DbTypes dbType);

    bool HasDbConnection();
}
