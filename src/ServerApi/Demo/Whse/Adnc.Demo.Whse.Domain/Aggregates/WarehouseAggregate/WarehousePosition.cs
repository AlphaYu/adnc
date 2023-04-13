namespace Adnc.Demo.Whse.Domain.Aggregates.WarehouseAggregate;

public record WarehousePosition : ValueObject
{
    public string Code { get; }

    public string Description { get; }

    private WarehousePosition()
    {
    }

    internal WarehousePosition(string code, string description)
    {
        this.Code = Guard.Checker.NotNullOrEmpty(code, nameof(code));
        this.Description = description is null ? string.Empty : description.Trim();
    }
}