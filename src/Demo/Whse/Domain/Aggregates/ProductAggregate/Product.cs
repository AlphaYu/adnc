namespace Adnc.Demo.Whse.Domain.Aggregates.ProductAggregate;

public class Product : AggregateRootWithBasicAuditInfo
{
    public static readonly int Name_MaxLength = 64;
    public static readonly int Describe_MaxLength = 128;
    public static readonly int Sku_MaxLength = 32;
    public static readonly int ChangesReason_MaxLength = 32;
    public static readonly int Unit_MaxLength = 4;

    private Product()
    {
        Status = new ProductStatus(ProductStatusCodes.UnKnow, string.Empty);
    }

    /// <summary>
    /// Product creation must rely on the repository to check for duplicate names, so part of the business logic must stay in the domain service.
    /// internal prevents application services from directly using Product constructors and requires ProductManager to create instances.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sku"></param>
    /// <param name="price"></param>
    /// <param name="name"></param>
    /// <param name="unit"></param>
    /// <param name="describe"></param>
    internal Product(long id, string sku, decimal price, string name, string unit, string? describe = null)
    {
        Id = id;
        SetSku(sku);
        SetName(name);
        SetPrice(price);
        SetUnit(unit);
        Describe = describe != null ? describe.Trim() : string.Empty;
        Status = new ProductStatus(ProductStatusCodes.UnKnow, string.Empty);
    }

    public string Sku { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string Describe { set; get; } = string.Empty;

    public decimal Price { get; private set; }

    public ProductStatus Status { get; private set; }

    public string Unit { get; private set; } = string.Empty;

    /// <summary>
    /// Set the SKU
    /// </summary>
    /// <param name="sku"></param>
    internal void SetSku(string sku)
    {
        Sku = Checker.Variable.NotNullOrWhiteSpace(sku.Trim(), nameof(sku));
    }

    /// <summary>
    /// Set the name
    /// </summary>
    /// <param name="name"></param>
    internal void SetName(string name)
    {
        Name = Checker.Variable.NotNullOrWhiteSpace(name.Trim(), nameof(name));
    }

    /// <summary>
    /// Set the product status
    /// </summary>
    /// <param name="status"></param>
    internal void SetStatus(ProductStatus status)
    {
        Status = Checker.Variable.NotNull(status, nameof(status));
    }

    /// <summary>
    /// Change the unit
    /// </summary>
    /// <param name="unit"></param>
    public void SetUnit(string unit)
    {
        Unit = Checker.Variable.NotNullOrWhiteSpace(unit, nameof(unit));
    }

    /// <summary>
    /// Change the price
    /// </summary>
    /// <param name="price"></param>
    public void SetPrice(decimal price)
    {
        Checker.Variable.GTZero(price, nameof(price));
        Price = price;
    }

    /// <summary>
    /// Take a product off sale so it cannot be sold
    /// </summary>
    public void PutOffSale(string reason)
    {
        Status = new ProductStatus(ProductStatusCodes.SaleOff, reason);
    }
}
