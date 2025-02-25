namespace Microsoft.EntityFrameworkCore;

public static class DbContextExtension
{
    public static string? GetTableName<TEntity>(this DbContext dbContext)
    {
        var entityType = dbContext.Model.FindEntityType(typeof(TEntity));
        return entityType?.GetTableName();
    }

    public static string? GetGetSchema<TEntity>(this DbContext dbContext)
    {
        var entityType = dbContext.Model.FindEntityType(typeof(TEntity));
        return entityType?.GetSchema();
    }

    public static EntityEntry<TEntity> GetEntityEntry<TEntity>(this DbContext dbContext, TEntity entity, out bool existBefore)
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
            .FirstOrDefault(x => EFCoreUtil.GetEntityKeyValues(keysGetter, x).SequenceEqual(keyValues));

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
}