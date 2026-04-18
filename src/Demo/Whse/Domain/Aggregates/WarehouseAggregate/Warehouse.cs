namespace Adnc.Demo.Whse.Domain.Aggregates.WarehouseAggregate;

/// <summary>
/// Warehouse shelf
/// </summary>
public class Warehouse : AggregateRootWithBasicAuditInfo
{
    private Warehouse()
    {
    }

    internal Warehouse(long id, WarehousePosition position)
    {
        Id = id;
        Qty = 0;
        BlockedQty = 0;
        Position = Checker.Variable.NotNull(position, nameof(position));
    }

    public long ProductId { get; private set; }

    public int Qty { get; private set; }

    public int BlockedQty { get; private set; }

    public WarehousePosition Position { get; private set; } = default!;

    /// <summary>
    /// Reserve inventory
    /// </summary>
    /// <param name="needBlockedQty"></param>
    internal void BlockQty(int needBlockedQty)
    {
        if (Qty < needBlockedQty)
        {
            throw new BusinessException("Qty<needFreezedQty");
        }

        Checker.Variable.GTZero(ProductId, nameof(ProductId));

        BlockedQty += needBlockedQty;
        Qty -= needBlockedQty;
    }

    /// <summary>
    /// Remove reserved inventory
    /// </summary>
    /// <param name="needRemoveQty"></param>
    internal void RemoveBlockedQty(int needRemoveQty)
    {
        if (BlockedQty < needRemoveQty)
        {
            throw new BusinessException("FreezedQty<needUnfreezeQty");
        }

        Checker.Variable.GTZero(ProductId, nameof(ProductId));

        BlockedQty -= needRemoveQty;
        Qty += needRemoveQty;
    }

    /// <summary>
    /// Ship inventory
    /// </summary>
    /// <param name="qty"></param>
    internal void Deliver(int qty)
    {
        if (BlockedQty < qty)
        {
            throw new BusinessException("FreezedQty<qty");
        }

        Checker.Variable.GTZero(ProductId, nameof(ProductId));

        BlockedQty -= qty;
    }

    /// <summary>
    /// Stock inventory
    /// </summary>
    /// <param name="qty"></param>
    internal void Entry(int qty)
    {
        Checker.Variable.GTZero(qty, nameof(qty));
        Checker.Variable.GTZero(ProductId, nameof(ProductId));
        Qty += qty;
    }

    /// <summary>
    /// Assign the shelf to a product
    /// </summary>
    /// <param name="productId"></param>
    internal void SetProductId(long productId)
    {
        //if (this.ProductId.HasValue && this.ProductId == productId)
        //    throw new ArgumentException("ProductId");

        Checker.Variable.GTZero(productId, nameof(productId));

        ProductId = productId;
    }
}
