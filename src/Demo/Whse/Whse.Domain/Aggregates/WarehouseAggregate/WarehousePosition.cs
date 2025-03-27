namespace Adnc.Demo.Whse.Domain.Aggregates.WarehouseAggregate;

public record WarehousePosition : ValueObject
{
    public string Code { get; } = string.Empty;

    public string Description { get; } = string.Empty;

    private WarehousePosition()
    {
    }

    internal WarehousePosition(string code, string description)
    {
        Code = Checker.Variable.NotNullOrWhiteSpace(code, nameof(code));
        Description = description is null ? string.Empty : description.Trim();
    }
}