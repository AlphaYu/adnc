namespace Adnc.Demo.Whse.Application.Services.Implements;

/// <summary>
///  商品管理
/// </summary>
/// <remarks>
/// 商品管理构造函数
/// </remarks>
/// <param name="productRepo"></param>
/// <param name="adminClient"></param>
/// <param name="productMgr"></param>
public class ProductService(IEfBasicRepository<Product> productRepo, /*IEfBasicRepository<Warehouse> warehouseRepo,*/ IAdminRestClient adminClient, ProductManager productMgr)
    : AbstractAppService, IProductService
{
    /// <summary>
    /// 创建商品
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ProductDto> CreateAsync(ProductCreationDto input)
    {
        input.TrimStringFields();
        var product = await productMgr.CreateAsync(input.Sku, input.Price, input.Name, input.Unit, input.Describe);

        await productRepo.InsertAsync(product);

        return Mapper.Map<ProductDto>(product);
    }

    /// <summary>
    /// 修改商品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ProductDto> UpdateAsync(long id, ProductUpdationDto input)
    {
        input.TrimStringFields();
        var product = await productRepo.GetRequiredAsync(id);
        product.Describe = input.Describe;
        product.SetUnit(input.Unit);
        product.SetPrice(input.Price);

        await productMgr.ChangeSkuAsync(product, input.Sku);
        await productMgr.ChangeNameAsync(product, input.Name);

        await productRepo.UpdateAsync(product);

        return Mapper.Map<ProductDto>(product);
    }

    /// <summary>
    /// 调整价格
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ProductDto> ChangePriceAsync(long id, ProducChangePriceDto input)
    {
        input.TrimStringFields();
        var product = await productRepo.GetRequiredAsync(id);

        product.SetPrice(input.Price);

        await productRepo.UpdateAsync(product);

        return Mapper.Map<ProductDto>(product);
    }

    /// <summary>
    /// 上架商品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ProductDto> PutOnSaleAsync(long id, ProductPutOnSaleDto input)
    {
        input.TrimStringFields();
        var product = await productRepo.GetRequiredAsync(id);
        //var warehouseInfo = await warehouseRepo.Where(x => x.ProductId == id).FirstOrDefaultAsync();

        productMgr.PutOnSale(product, input.Reason);

        await productRepo.UpdateAsync(product);

        return Mapper.Map<ProductDto>(product);
    }

    /// <summary>
    /// 下架商品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ProductDto> PutOffSaleAsync(long id, ProductPutOffSaleDto input)
    {
        input.TrimStringFields();
        var product = await productRepo.GetRequiredAsync(id);

        product.PutOffSale(input.Reason);

        await productRepo.UpdateAsync(product);

        return Mapper.Map<ProductDto>(product);
    }

    /// <summary>
    /// 商品分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PageModelDto<ProductDto>> GetPagedAsync(ProductSearchPagedDto input)
    {
        input.TrimStringFields();
        var whereCondition = ExpressionCreator
                                            .New<Product>()
                                            .AndIf(input.Id > 0, x => x.Id == input.Id);

        var total = await productRepo.CountAsync(whereCondition);
        if (total == 0)
        {
            return new PageModelDto<ProductDto>(input);
        }

        var entities = await productRepo
                            .Where(whereCondition)
                            .OrderByDescending(x => x.Id)
                            .Skip(input.SkipRows())
                            .Take(input.PageSize)
                            .ToListAsync();

        var productDtos = Mapper.Map<List<ProductDto>>(entities);
        if (productDtos.IsNotNullOrEmpty())
        {
            //调用maint微服务获取字典,组合商品状态信息
            var productStatus = (await adminClient.GetDictOptionsAsync("product_status")).FirstOrDefault();
            if (productStatus is not null)
            {
                productDtos.ForEach(x =>
                {
                    x.StatusDescription = productStatus.DictDataList.FirstOrDefault(d => d.Value == x.StatusCode.ToString())?.Label ?? string.Empty;
                });
            }
        }

        return new PageModelDto<ProductDto>(input, productDtos, total);
    }

    /// <summary>
    /// 商品列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<ProductDto>> GetListAsync(ProductSearchListDto input)
    {
        input.TrimStringFields();
        var whereCondition = ExpressionCreator
                                            .New<Product>()
                                            .AndIf(input.Ids.IsNotNullOrEmpty(), x => input.Ids.Select(x => x).Distinct().Contains(x.Id))
                                            .AndIf(input.StatusCode > 0, x => (int)x.Status.Code == input.StatusCode);

        var products = await productRepo.Where(whereCondition).ToListAsync();
        var productsDto = Mapper.Map<List<ProductDto>>(products);

        return productsDto;
    }
}
