namespace Adnc.Infra.Repository.EfCore.SqlServer;

public class SqlServerDbContext : AdncDbContext
{
    public SqlServerDbContext(
        DbContextOptions options,
        IEntityInfo entityInfo)
        : base(options, entityInfo)
    {
    }
}