using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Adnc.Infra.Repository
{
    /// <summary>
    /// An entity in a MongoDB repository.
    /// </summary>
    public abstract class MongoEntity : Entity, IEfEntity<long>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        //_id 由StringObjectIdGenerator 生成，存储类型是string
        //[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        //告诉mongodb这个字段在数据库中的类型是ObjectId,加这个特性是为了解决nlog生成日志时，存入的是objectid类型。
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public long Id { get; set; }
    }
}