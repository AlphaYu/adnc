namespace Adnc.Infra.Repository.EfCore;

public class MySQLDbContext : AdncDbContext
{
    public MySQLDbContext(
        DbContextOptions options
        , Operater operater
        , IEntityInfo entityInfo)
        : base(options, operater, entityInfo)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //System.Diagnostics.Debugger.Launch();
        modelBuilder.HasCharSet("utf8mb4 ");
        base.OnModelCreating(modelBuilder);
    }
}