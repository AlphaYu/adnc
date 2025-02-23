using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Adnc.Infra.Repository
{
    public class Entity : IEntity<long>
    {
        public long Id { get; set; }
    }
}