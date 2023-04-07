using Adnc.Infra.Entities;
using Adnc.Infra.IRepositories;
using Adnc.Infra.Repository.Mongo.Extensions;
using Adnc.Infra.Repository.Mongo.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Adnc.Infra.Repository.Mongo
{
    /// <summary>
    /// A MongoDB based repository of <see cref="T:TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class MongoRepository<TEntity> : IMongoRepository<TEntity>
        where TEntity : MongoEntity
    {
        private readonly IMongoContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MongoRepository(IMongoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default) =>
            await FindOneAsync(Filter.IdEq(id), null, cancellationToken);

        public async Task<TEntity> GetAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default) =>
           await FindOneAsync(filter, null, cancellationToken);

        /// <summary>
        /// Gets all entities in this repository.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await FindAsync(Filter.Empty, null, cancellationToken);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var collection = await GetCollectionAsync(cancellationToken);
            await collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Adds the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual async Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var collection = await GetCollectionAsync(cancellationToken);
            await collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Deletes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var collection = await GetCollectionAsync(cancellationToken);
            var result = await collection.FindOneAndDeleteAsync(Filter.IdEq(id), cancellationToken: cancellationToken);
            return result?.Id == id;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="filter">删除条件</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual async Task<long> DeleteManyAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
        {
            var collection = await GetCollectionAsync(cancellationToken);
            var result = await collection.DeleteManyAsync(filter, cancellationToken);
            return result.DeletedCount;
        }

        /// <summary>
        /// Replaces the specified entity with the same identifier.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The replaced document.</returns>
        public virtual async Task<TEntity> ReplaceAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var collection = await GetCollectionAsync(cancellationToken);
            return await collection.FindOneAndReplaceAsync(
                Filter.IdEq(entity.Id), entity,
                new FindOneAndReplaceOptions<TEntity> { ReturnDocument = ReturnDocument.After },
                cancellationToken);
        }

        public virtual async Task<PagedModel<TEntity>> PagedAsync(int pageIndex, int pageSize, FilterDefinition<TEntity> filter, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, CancellationToken cancellationToken = default)
        {
            var collection = await GetCollectionAsync(cancellationToken);
            var total = await collection.CountDocumentsAsync(filter);
            if (total == 0)
            {
                return new PagedModel<TEntity>() { PageSize = pageSize };
            }

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            var sortModel = ascending
                            ? Sort.Ascending(orderByExpression)
                            : Sort.Descending(orderByExpression)
                            ;

            var data = await collection.Find(filter)
            .Sort(sortModel)
            .Skip((pageIndex - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

            return new PagedModel<TEntity>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total,
                Data = data
            };
        }

        /// <summary>
        /// Updates the entity with the specified key according to the specified update definition.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="updateDefinition">The update definition.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected async Task<TEntity> UpdateAsync(string id, UpdateDefinition<TEntity> updateDefinition, FindOneAndUpdateOptions<TEntity>? options = null, CancellationToken cancellationToken = default)
        {
            var collection = await GetCollectionAsync(cancellationToken);
            return await collection.FindOneAndUpdateAsync(Filter.IdEq(id), updateDefinition, options, cancellationToken);
        }

        /// <summary>
        /// Finds the entity according to the specified filter definition.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected async Task<TEntity> FindOneAsync(FilterDefinition<TEntity> filter, FindOptions<TEntity>? options = null, CancellationToken cancellationToken = default)
        {
            var collection = await GetCollectionAsync(cancellationToken);
            var cursor = await collection.FindAsync(filter, options, cancellationToken);
            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Finds all entities according to the specified filter definition.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected async Task<ICollection<TEntity>> FindAsync(FilterDefinition<TEntity> filter, FindOptions<TEntity>? options = null, CancellationToken cancellationToken = default)
        {
            var collection = await GetCollectionAsync(cancellationToken);
            var cursor = await collection.FindAsync(filter, options, cancellationToken);
            return await cursor.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the Mongo collection that backs this repository.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected async Task<IMongoCollection<TEntity>> GetCollectionAsync(CancellationToken cancellationToken = default) =>
            await _context.GetCollectionAsync<TEntity>(cancellationToken);

        protected static FilterDefinitionBuilder<TEntity> Filter => Builders<TEntity>.Filter;

        protected static SortDefinitionBuilder<TEntity> Sort => Builders<TEntity>.Sort;

        protected static UpdateDefinitionBuilder<TEntity> Update => Builders<TEntity>.Update;

        protected static ProjectionDefinitionBuilder<TEntity> Projection => Builders<TEntity>.Projection;
    }
}