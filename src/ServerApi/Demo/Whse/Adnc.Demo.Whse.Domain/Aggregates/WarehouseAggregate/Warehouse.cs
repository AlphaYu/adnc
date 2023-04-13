namespace Adnc.Demo.Whse.Domain.Aggregates.WarehouseAggregate;

/// <summary>
/// 货架
/// </summary>
public class Warehouse : AggregateRootWithBasicAuditInfo
{
    public long? ProductId { get; private set; }

    public int Qty { get; private set; }

    public int BlockedQty { get; private set; }

    public WarehousePosition Position { get; private set; }

    private Warehouse()
    {
    }

    internal Warehouse(long id, WarehousePosition position)
    {
        this.Id = id;
        this.Qty = 0;
        this.BlockedQty = 0;
        this.Position = Guard.Checker.NotNull(position, nameof(position));
    }

    /// <summary>
    /// 冻结库存
    /// </summary>
    /// <param name="needBlockedQty"></param>
    internal void BlockQty(int needBlockedQty)
    {
        if (this.Qty < needBlockedQty)
            throw new BusinessException("Qty<needFreezedQty");

        Guard.Checker.GTZero(ProductId.Value, nameof(ProductId));

        this.BlockedQty += needBlockedQty;
        this.Qty -= needBlockedQty;
    }

    /// <summary>
    /// 移除被冻结的库存
    /// </summary>
    /// <param name="needRemoveQty"></param>
    internal void RemoveBlockedQty(int needRemoveQty)
    {
        if (this.BlockedQty < needRemoveQty)
            throw new BusinessException("FreezedQty<needUnfreezeQty");

        Guard.Checker.GTZero(ProductId.Value, nameof(ProductId));

        this.BlockedQty -= needRemoveQty;
        this.Qty += needRemoveQty;
    }

    /// <summary>
    /// 出库
    /// </summary>
    /// <param name="qty"></param>
    internal void Deliver(int qty)
    {
        if (this.BlockedQty < qty)
            throw new BusinessException("FreezedQty<qty");

        Guard.Checker.GTZero(ProductId.Value, nameof(ProductId));

        this.BlockedQty -= qty;
    }

    /// <summary>
    /// 入库
    /// </summary>
    /// <param name="qty"></param>
    internal void Entry(int qty)
    {
        Guard.Checker.GTZero(qty, nameof(qty));
        Guard.Checker.GTZero(ProductId.Value, nameof(ProductId));
        this.Qty += qty;
    }

    /// <summary>
    /// 分配货架给商品
    /// </summary>
    /// <param name="productId"></param>
    internal void SetProductId(long productId)
    {
        //if (this.ProductId.HasValue && this.ProductId == productId)
        //    throw new ArgumentException("ProductId");

        Guard.Checker.GTZero(productId, nameof(productId));

        this.ProductId = productId;
    }
}