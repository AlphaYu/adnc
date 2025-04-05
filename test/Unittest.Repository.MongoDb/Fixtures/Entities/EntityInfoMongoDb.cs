using Adnc.Infra.Repository.EfCore.MongoDB;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Adnc.Infra.Unittest.Reposity.MongoDb.Fixtures.Entities;

public class MongoEntityInfo : AbstractMongoDbEntityInfo
{
    protected override List<Assembly> GetEntityAssemblies() => [GetType().Assembly];

    protected override void SetCollectionName(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoggerLog>().ToCollection("logger_log");
    }
}
