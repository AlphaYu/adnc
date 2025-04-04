namespace Adnc.Infra.Repository.EfCore.MongoDB;

public class MongoDbContext(DbContextOptions options, IEntityInfo entityInfo, Operater operater) : AdncDbContext(options, entityInfo, operater)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;
    }
}
