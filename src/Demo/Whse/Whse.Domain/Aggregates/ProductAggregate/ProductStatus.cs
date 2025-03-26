namespace Adnc.Demo.Whse.Domain.Aggregates.ProductAggregate;

public record ProductStatus : ValueObject
{
    public ProductStatusCodes Code { get; }

    public string ChangesReason { get; } = string.Empty;

    private ProductStatus()
    {
    }

    internal ProductStatus(ProductStatusCodes statusCode, string reason)
    {
        Code = statusCode;
        ChangesReason = reason is null ? string.Empty : reason.Trim();
    }
}