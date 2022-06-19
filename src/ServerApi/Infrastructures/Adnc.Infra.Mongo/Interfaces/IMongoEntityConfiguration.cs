using Adnc.Infra.Entities;
using Adnc.Infra.Repository.Mongo.Configuration;

namespace Adnc.Infra.Repository.Mongo.Interfaces
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