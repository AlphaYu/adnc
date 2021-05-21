using Adnc.Core.Shared.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Adnc.Infra.Mongo.Models
{
    /// <summary>
    /// A mongo entity with soft delete support.
    /// </summary>
    public abstract class SoftDeletableMongoEntity : MongoEntity
    {
        /// <summary>
        /// Gets or sets the date that this entity was soft deleted.
        /// Or null if it was not soft deleted.
        /// </summary>
        [BsonIgnoreIfNull]
        public DateTime? DateDeleted { get; set; }
    }
}