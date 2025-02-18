namespace Adnc.Infra.Repository.EfCore.SqlServer;

public class SqlServerDbContext(DbContextOptions options, IEntityInfo entityInfo) : AdncDbContext(options, entityInfo)
{
}