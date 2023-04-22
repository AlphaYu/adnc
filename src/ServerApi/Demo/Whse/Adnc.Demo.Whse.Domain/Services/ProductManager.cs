using Adnc.Demo.Whse.Domain.Aggregates;
using Adnc.Demo.Whse.Domain.Aggregates.ProductAggregate;

namespace Adnc.Demo.Whse.Domain.Services;

public class ProductManager : IDomainService
{
    private readonly IEfBasicRepository<Product> _productRepo;

    public ProductManager(IEfBasicRepository<Product> productRepo) => _productRepo = productRepo;

    /// <summary>
    /// 创建商品
    /// </summary>
    /// <param name="sku"></param>
    /// <param name="price"></param>
    /// <param name="name"></param>
    /// <param name="unit"></param>
    /// <param name="describe"></param>
    /// <returns></returns>
    public virtual async Task<Product> CreateAsync(string sku, decimal price, string name, string unit, string describe = null)
    {
        var exists = await _productRepo.AnyAsync(x => x.Sku == sku);
        if (exists)
            throw new BusinessException($"sku exists({sku})");

        exists = await _productRepo.AnyAsync(x => x.Name == name);
        if (exists)
            throw new BusinessException($"name exists{name}");

        return new Product(
                            IdGenerater.GetNextId()
                            , sku
                            , price
                            , name
                            , unit
                            , describe
                            );
    }

    /// <summary>
    /// 修改SKU
    /// </summary>
    /// <param name="product"></param>
    /// <param name="newSku"></param>
    /// <returns></returns>
    public virtual async Task ChangeSkuAsync(Product product, string newSku)
    {
        Guard.Checker.NotNull(product, nameof(product));

        if (product.Sku.EqualsIgnoreCase(newSku))
            return;

        var exists = await _productRepo.AnyAsync(x => x.Sku == newSku);
        if (exists)
            throw new BusinessException($"sku exists({newSku})");

        product.SetSku(newSku);
    }

    /// <summary>
    /// 修改商品名称
    /// </summary>
    /// <param name="product"></param>
    /// <param name="newName"></param>
    /// <returns></returns>
    public virtual async Task ChangeNameAsync(Product product, string newName)
    {
        Guard.Checker.NotNull(product, nameof(product));

        if (product.Name.EqualsIgnoreCase(newName))
            return;

        var exists = await _productRepo.AnyAsync(x => x.Name == newName);
        if (exists)
            throw new BusinessException($"name exists{newName}");

        product.SetName(newName);
    }

    /// <summary>
    /// 上架商品
    /// </summary>
    /// <param name="product"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public virtual void PutOnSale(Product product, string reason)
    {
        Guard.Checker.NotNull(product, nameof(product));
        product.SetStatus(new ProductStatus(ProductStatusCodes.SaleOn, reason));
        //if (warehouseInfo.Qty > 0 && warehouseInfo.ProductId == product.Id)
        //    product.SetStatus(new ProductStatus(ProductStatusCodes.SaleOn, reason));
        //else
        //    throw new BusinessException($"product exists{product.Id}");
    }
}