namespace Adnc.Infra.Repository;

public interface IAdoRepository : IRepository
{
    void ChangeOrSetDbConnection(IDbConnection dbConnection);

    IDbConnection CreateDbConnection(string connectionString, DbTypes dbType);

    bool HasDbConnection();
}
