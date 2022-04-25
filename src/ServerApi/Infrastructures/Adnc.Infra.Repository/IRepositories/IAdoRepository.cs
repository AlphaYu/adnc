namespace Adnc.Infra.IRepositories;

public interface IAdoRepository
{
    void ChangeOrSetDbConnection(IDbConnection dbConnection);
    void ChangeOrSetDbConnection(string connectionString, DbTypes dbType);
    bool HasDbConnection();
}
