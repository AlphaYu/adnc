using Adnc.Infra.Repository.EfCore.MongoDB;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Adnc.Infra.Unittest.Reposity.MongoDb.Fixtures.Entities;

public class MongoEntityInfo : AbstractMongoDbEntityInfo
{
    protected override List<Assembly> GetCurrentAssemblies() => [GetType().Assembly];

    protected override void SetCollectionName(dynamic modelBuilder)
    {
        if (modelBuilder is not ModelBuilder builder)
            throw new ArgumentNullException(nameof(modelBuilder));

        builder.Entity<LoggerLog>().ToCollection("logger_log");
    }
}