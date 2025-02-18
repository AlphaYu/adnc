namespace Adnc.Infra.Repository.EfCore.MySql;

public class MySqlDbContext(DbContextOptions options, IEntityInfo entityInfo) : AdncDbContext(options, entityInfo)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //System.Diagnostics.Debugger.Launch();
        modelBuilder.HasCharSet("utf8mb4 ");
        base.OnModelCreating(modelBuilder);
    }
}