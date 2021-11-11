﻿using Adnc.Infra.IRepositories;
using Adnc.Infra.Mongo.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Infra.Mongo.Interfaces
{
    /// <summary>
    /// A MongoDB based repository of <see cref="TEntity" /> that supports soft deletion.
    /// Entities that implement soft deletion should probably have an index defined on the <see cref="SoftDeletableMongoEntity.DateDeleted"/> field.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="IMongoRepository{TEntity}" />
    public interface ISoftDeletableMongoRepository<TEntity> : IMongoRepository<TEntity>
        where TEntity : SoftDeletableMongoEntity
    {
        /// <summary>
        /// Un-deletes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> UnDeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}