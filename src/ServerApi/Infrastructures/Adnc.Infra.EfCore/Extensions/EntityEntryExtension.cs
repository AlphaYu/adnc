using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public static class EntityEntryExtension
{
    internal static KeyEntryModel[]? GetKeyValues(this EntityEntry entry)
    {
        if (!entry.IsKeySet)
            return default;

        var keyProps = entry.Properties
                                    .Where(p => p.Metadata.IsPrimaryKey())
                                    .ToArray();
        if (keyProps.IsNullOrEmpty())
            return default;

        var keyEntries = new KeyEntryModel[keyProps.Length];
        if (keyEntries.IsNullOrEmpty())
            return default;

        for (var i = 0; i < keyProps.Length; i++)
        {
            keyEntries[i] = new KeyEntryModel()
            {
                PropertyName = keyProps[i].Metadata.Name,
                ColumnName = (keyProps[i].Metadata as PropertyEntry)?.GetColumnName() ?? string.Empty,
                Value = keyProps[i]?.CurrentValue ?? string.Empty,
            };
        }

        return keyEntries;
    }

    public static string? GetColumnName(this PropertyEntry entry)
    {
        var storeObjectId = StoreObjectIdentifier.Create(entry.Metadata.DeclaringEntityType, StoreObjectType.Table);
        return entry.Metadata.GetColumnName(storeObjectId.GetValueOrDefault());
    }
}