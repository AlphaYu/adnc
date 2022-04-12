namespace Microsoft.EntityFrameworkCore;

public static class DbContextExtension
{
    public static string GetTableName<TEntity>([NotNull] this DbContext @this)
    {
        var entityType = @this.Model.FindEntityType(typeof(TEntity));
        return entityType?.GetTableName();
    }

    public static string GetGetSchema<TEntity>([NotNull] this DbContext @this)
    {
        var entityType = @this.Model.FindEntityType(typeof(TEntity));
        return entityType?.GetSchema();
    }

    public static EntityEntry<TEntity> GetEntityEntry<TEntity>([NotNull] this DbContext @this, TEntity entity, out bool existBefore)
    where TEntity : class
    {
        var type = typeof(TEntity);

        var entityType = @this.Model.FindEntityType(type);

        var keysGetter = entityType.FindPrimaryKey().Properties
            .Select(x => x.PropertyInfo.GetValueGetter<TEntity>())
            .ToArray();

        var keyValues = keysGetter
            .Select(x => x.Invoke(entity))
            .ToArray();

        var originalEntity = @this.Set<TEntity>().Local
            .FirstOrDefault(x => EFCoreUtil.GetEntityKeyValues(keysGetter, x).SequenceEqual(keyValues));

        EntityEntry<TEntity> entityEntry;
        if (null == originalEntity)
        {
            existBefore = false;
            entityEntry = @this.Attach(entity);
        }
        else
        {
            existBefore = true;
            entityEntry = @this.Entry(originalEntity);
            entityEntry.CurrentValues.SetValues(entity);
        }

        return entityEntry;
    }
}