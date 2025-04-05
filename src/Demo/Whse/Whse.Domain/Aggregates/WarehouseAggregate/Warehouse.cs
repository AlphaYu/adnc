namespace Adnc.Demo.Whse.Domain.Aggregates.WarehouseAggregate;

/// <summary>
/// 货架
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
    /// 冻结库存
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
    /// 移除被冻结的库存
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
    /// 出库
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
    /// 入库
    /// </summary>
    /// <param name="qty"></param>
    internal void Entry(int qty)
    {
        Checker.Variable.GTZero(qty, nameof(qty));
        Checker.Variable.GTZero(ProductId, nameof(ProductId));
        Qty += qty;
    }

    /// <summary>
    /// 分配货架给商品
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
