using Adnc.Infra.Entities;
using MongoDB.Driver;
using System.Collections;

namespace Adnc.Infra.Repository.Mongo.Configuration
{
    /// <summary>
    /// A collection of mongo indexes.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public sealed class MongoIndexContext<TEntity> : ICollection<CreateIndexModel<TEntity>>
        where TEntity : MongoEntity
    {
        private readonly ICollection<CreateIndexModel<TEntity>> _indexes = new List<CreateIndexModel<TEntity>>();

        /// <inheritdoc />
        public IEnumerator<CreateIndexModel<TEntity>> GetEnumerator() => _indexes.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void Add(CreateIndexModel<TEntity> item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var name = item.Options.Name;

            if (_indexes.Any(x => x.Options.Name == name))
            {
                throw new ArgumentException($"An index with the name {name} has already been added", nameof(name));
            }

            _indexes.Add(item);
        }

        /// <inheritdoc />
        public void Clear() => _indexes.Clear();

        /// <inheritdoc />
        public bool Contains(CreateIndexModel<TEntity> item) => _indexes.Contains(item);

        /// <inheritdoc />
        public void CopyTo(CreateIndexModel<TEntity>[] array, int arrayIndex) => _indexes.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(CreateIndexModel<TEntity> item) => _indexes.Remove(item);

        /// <inheritdoc />
        public int Count => _indexes.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;
    }
}