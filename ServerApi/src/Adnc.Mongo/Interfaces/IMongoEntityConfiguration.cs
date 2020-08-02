using Adnc.Infr.Mongo.Configuration;
using Adnc.Infr.Mongo.Models;
using Adnc.Core.Entities;

namespace Adnc.Infr.Mongo.Interfaces
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