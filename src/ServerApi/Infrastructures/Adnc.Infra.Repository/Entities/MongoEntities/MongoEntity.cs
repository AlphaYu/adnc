using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Adnc.Infra.Entities
{
    /// <summary>
    /// An entity in a MongoDB repository.
    /// </summary>
    public abstract class MongoEntity : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        //_id 由StringObjectIdGenerator 生成，存储类型是string
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        //告诉mongodb这个字段在数据库中的类型是ObjectId,加这个特性是为了解决nlog生成日志时，存入的是objectid类型。
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; } = default!;
    }
}