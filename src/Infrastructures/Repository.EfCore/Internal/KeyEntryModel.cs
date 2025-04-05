namespace Adnc.Infra.Repository.EfCore.Internal;

internal sealed class KeyEntryModel
{
    public string PropertyName { get; set; } = string.Empty;

    public string ColumnName { get; set; } = string.Empty;

    public object Value { get; set; } = default!;
}
