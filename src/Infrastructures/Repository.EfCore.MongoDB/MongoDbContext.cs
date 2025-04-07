namespace Adnc.Infra.Repository.EfCore.MongoDB;

public class MongoDbContext(DbContextOptions options, IEntityInfo entityInfo, Operater operater) : AdncDbContext(options, entityInfo, operater)
{ }
