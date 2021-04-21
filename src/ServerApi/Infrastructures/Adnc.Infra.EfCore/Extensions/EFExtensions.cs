using System;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Adnc.Infra.EfCore.Extensions
{
    public static class EFExtensions
    {
        public static bool IsRelational([NotNull] this DatabaseFacade database)
        {
            if (null == database)
            {
                throw new ArgumentNullException(nameof(database));
            }

            return database.IsRelational();
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        public static string GetTableName<TEntity>(this DbContext dbContext)
        {
            var entityType = dbContext.Model.FindEntityType(typeof(TEntity));
            return entityType?.GetTableName();
        }

        public static string GetGetSchema<TEntity>(this DbContext dbContext)
        {
            var entityType = dbContext.Model.FindEntityType(typeof(TEntity));
            return entityType?.GetSchema();
        }

        public static KeyEntry[] GetKeyValues([NotNull] this EntityEntry entityEntry)
        {
            if (!entityEntry.IsKeySet)
                return null;

            var keyProps = entityEntry.Properties
                .Where(p => p.Metadata.IsPrimaryKey())
                .ToArray();
            if (keyProps.Length == 0)
                return null;

            var keyEntries = new KeyEntry[keyProps.Length];
            for (var i = 0; i < keyProps.Length; i++)
            {
                keyEntries[i] = new KeyEntry()
                {
                    PropertyName = keyProps[i].Metadata.Name,
                    ColumnName = keyProps[i].Metadata.GetColumnName(),
                    Value = keyProps[i].CurrentValue,
                };
            }

            return keyEntries;
        }

        public static EntityEntry<TEntity> GetEntityEntry<TEntity>([NotNull] this DbContext dbContext, TEntity entity, out bool existBefore)
        where TEntity : class
        {
            var type = typeof(TEntity);

            var entityType = dbContext.Model.FindEntityType(type);

            var keysGetter = entityType.FindPrimaryKey().Properties
                .Select(x => x.PropertyInfo.GetValueGetter<TEntity>())
                .ToArray();

            var keyValues = keysGetter
                .Select(x => x.Invoke(entity))
                .ToArray();

            var originalEntity = dbContext.Set<TEntity>().Local
                .FirstOrDefault(x => GetEntityKeyValues(keysGetter, x).SequenceEqual(keyValues));

            EntityEntry<TEntity> entityEntry;
            if (null == originalEntity)
            {
                existBefore = false;
                entityEntry = dbContext.Attach(entity);
            }
            else
            {
                existBefore = true;
                entityEntry = dbContext.Entry(originalEntity);
                entityEntry.CurrentValues.SetValues(entity);
            }

            return entityEntry;
        }

        public static object[] GetEntityKeyValues<TEntity>(Func<TEntity, object>[] keyValueGetter, TEntity entity)
        {
            var keyValues = keyValueGetter.Select(x => x.Invoke(entity)).ToArray();
            return keyValues;
        }
    }

}
