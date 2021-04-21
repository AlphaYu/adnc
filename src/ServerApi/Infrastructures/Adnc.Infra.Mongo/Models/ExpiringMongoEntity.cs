using System;
using Adnc.Core.Shared.Entities;

namespace Adnc.Infra.Mongo.Models
{
    /// <summary>
    /// A mongo entity that will be automatically deleted after a configured time has elapsed.
    /// </summary>
    /// <seealso cref="MongoEntity" />
    public abstract class ExpiringMongoEntity : MongoEntity
    {
        /// <summary>
        /// Gets or sets the date at which this entity was created.
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
