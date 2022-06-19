namespace Adnc.Infra.Repository.EfCore.SqlServer;

public class SqlServerDbContext : AdncDbContext
{
    public SqlServerDbContext(
        DbContextOptions options
        , Operater operater
        , IEntityInfo entityInfo)
        : base(options, operater, entityInfo)
    {
    }
}