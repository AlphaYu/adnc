using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public static class EntityEntryExtension
{
    internal static KeyEntryModel[] GetKeyValues([NotNull] this EntityEntry @this)
    {
        if (!@this.IsKeySet)
            return default;

        var keyProps = @this.Properties
                                    .Where(p => p.Metadata.IsPrimaryKey())
                                    .ToArray();
        if (keyProps.Length == 0)
            return default;

        var keyEntries = new KeyEntryModel[keyProps.Length];
        for (var i = 0; i < keyProps.Length; i++)
        {
            keyEntries[i] = new KeyEntryModel()
            {
                PropertyName = keyProps[i].Metadata.Name,
                ColumnName = (keyProps[i].Metadata as PropertyEntry).GetColumnName(),
                Value = keyProps[i].CurrentValue,
            };
        }

        return keyEntries;
    }

    public static string GetColumnName(this PropertyEntry @this)
    {
        var storeObjectId = StoreObjectIdentifier.Create(@this.Metadata.DeclaringEntityType, StoreObjectType.Table);
        return @this.Metadata.GetColumnName(storeObjectId.GetValueOrDefault());
    }
}