namespace Adnc.Infra.Repository.EfCore.MySql;

public class MySqlDbContext : AdncDbContext
{
    public MySqlDbContext(
        DbContextOptions options,
        IEntityInfo entityInfo)
        : base(options, entityInfo)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //System.Diagnostics.Debugger.Launch();
        modelBuilder.HasCharSet("utf8mb4 ");
        base.OnModelCreating(modelBuilder);
    }
}