namespace Adnc.Infra.Repository.EfCore.Internal;

internal class KeyEntryModel
{
    public string PropertyName { get; set; } = default!;

    public string ColumnName { get; set; } = default!;

    public object Value { get; set; } = default!;
}