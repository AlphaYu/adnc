using Adnc.Infra.Mongo.Configuration;
using Adnc.Core.Shared.Entities;

namespace Adnc.Infra.Mongo.Interfaces
{
    /// <summary>
    /// Mongo entity configuration.
    /// </summary>
    public interface IMongoEntityConfiguration<TEntity>
        where TEntity : MongoEntity
    {
        void Configure(MongoEntityBuilder<TEntity> context);
    }
}