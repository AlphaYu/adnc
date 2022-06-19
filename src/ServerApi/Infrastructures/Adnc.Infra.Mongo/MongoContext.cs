using Adnc.Infra.Entities;
using Adnc.Infra.Repository.Mongo.Configuration;
using Adnc.Infra.Repository.Mongo.Extensions;
using Adnc.Infra.Repository.Mongo.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Concurrent;

namespace Adnc.Infra.Repository.Mongo
{
    /// <summary>
    /// Context used to maintain a single MongoDB connection.
    /// </summary>
    /// <seealso cref="IMongoContext" />
    /// <seealso cref="System.IDisposable" />
    public class MongoContext : IMongoContext, IDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly IOptions<MongoRepositoryOptions> _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMongoDatabase _database;
        private readonly ConcurrentBag<Type> _bootstrappedCollections = new ConcurrentBag<Type>();
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public MongoContext(IOptions<MongoRepositoryOptions> options, IServiceProvider serviceProvider)
        {
            _options = options;
            _serviceProvider = serviceProvider;

            var connectionString = options.Value.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Must provide a mongo connection string");
            }

            var url = new MongoUrl(connectionString);
            if (string.IsNullOrEmpty(url.DatabaseName))
            {
                throw new ArgumentNullException(nameof(connectionString), "Must provide a database name with the mongo connection string");
            }

            var clientSettings = MongoClientSettings.FromUrl(url);
            //clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());

            _semaphore = new SemaphoreSlim(1, 1);
            _database = new MongoClient(clientSettings).GetDatabase(url.DatabaseName);
        }

        /// <summary>
        /// Gets the MongoDB collection for the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<IMongoCollection<TEntity>> GetCollectionAsync<TEntity>(CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(MongoContext));
            }

            var collectionName = _options.Value.GetCollectionName<TEntity>();
            var collection = _database.GetCollection<TEntity>(collectionName);

            if (_bootstrappedCollections.Contains(typeof(TEntity)))
            {
                return collection;
            }

            try
            {
                await _semaphore.WaitAsync(cancellationToken);

                if (_bootstrappedCollections.Contains(typeof(TEntity)))
                {
                    return collection;
                }

                var configurations = _serviceProvider.GetServices<IMongoEntityConfiguration<TEntity>>();

                var builder = new MongoEntityBuilder<TEntity>();
                foreach (var configuration in configurations)
                {
                    configuration.Configure(builder);
                }

                // Indexes.
                var indexTasks = builder.Indexes.Select(index =>
                    collection.Indexes.CreateOneAsync(index, null, cancellationToken));
                await Task.WhenAll(indexTasks);

                // Seeds.
                var seedTasks = builder.Seed.Select(async seed =>
                {
                    var cursor = await collection.FindAsync(
                        Builders<TEntity>.Filter.IdEq(seed.Id),
                        cancellationToken: cancellationToken);

                    if (await cursor.AnyAsync(cancellationToken))
                    {
                        return;
                    }

                    await collection.InsertOneAsync(seed, cancellationToken: cancellationToken);
                });
                await Task.WhenAll(seedTasks);

                _bootstrappedCollections.Add(typeof(TEntity));

                return collection;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Drops the collection for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task DropCollectionAsync<TEntity>(CancellationToken cancellationToken = default)
        {
            var collectionName = _options.Value.GetCollectionName<TEntity>();
            await _database.DropCollectionAsync(collectionName, cancellationToken);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;

            _semaphore.Dispose();
        }
    }
}