using System;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Adnc.Whse.Core.Services;
using Adnc.Whse.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Application.Shared.Dtos;
using Adnc.Infra.Common.Extensions;
using Adnc.Application.Shared.RpcServices;
using Adnc.Infra.Common.Exceptions;
using Adnc.Application.Shared.Services;
using Adnc.Whse.Application.Contracts.Services;
using Adnc.Whse.Application.Contracts.Dtos;

namespace Adnc.Whse.Application.Services
{
    /// <summary>
    /// 商品管理
    /// </summary>
    public class ProductAppService : AbstractAppService, IProductAppService
    {
        private readonly ProductManager _productMgr;
        private readonly IEfBasicRepository<Product> _productRepo;
        private readonly IEfBasicRepository<Warehouse> _warehouseInfoRepo;
        private readonly IMaintRpcService _maintRpcSrv;

        /// <summary>
        /// 商品管理构造函数
        /// </summary>
        /// <param name="productRepo"></param>
        /// <param name="warehouseInfoRepo"></param>
        /// <param name="maintRpcSrv"></param>
        /// <param name="productMgr"></param>
        /// <param name="mapper"></param>
        public ProductAppService(
             IEfBasicRepository<Product> productRepo
            , IEfBasicRepository<Warehouse> warehouseInfoRepo
            , IMaintRpcService maintRpcSrv
            , ProductManager productMgr)
        {
            _productMgr = productMgr;
            _productRepo = productRepo;
            _warehouseInfoRepo = warehouseInfoRepo;
            _maintRpcSrv = maintRpcSrv;
        }

        /// <summary>
        /// 创建商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> CreateAsync(ProductCreationDto input)
        {
            var product = await _productMgr.CreateAsync(input.Sku, input.Price, input.Name, input.Unit, input.Describe);

            await _productRepo.InsertAsync(product);

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
            var product = await _productRepo.GetAsync(id);

            Checker.NotNull(product, nameof(product));

            product.Describe = input.Describe;
            product.SetUnit(input.Unit);
            product.SetPrice(input.Price);


            await _productMgr.ChangeSkuAsync(product, input.Sku);
            await _productMgr.ChangeNameAsync(product, input.Name);

            await _productRepo.UpdateAsync(product);

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
            var product = await _productRepo.GetAsync(id);

            Checker.NotNull(product, nameof(product));

            product.SetPrice(input.Price);

            await _productRepo.UpdateAsync(product);

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
            var product = await _productRepo.GetAsync(id);

            var warehouseInfo = await _warehouseInfoRepo.Where(x => x.ProductId == id).FirstOrDefaultAsync();

            _productMgr.PutOnSale(product, warehouseInfo, input.Reason);

            await _productRepo.UpdateAsync(product);

            return Mapper.Map<ProductDto>(product);
        }

        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> PutOffSaleAsync(long id, ProductPutOffSaleDto input)
        {
            var product = await _productRepo.GetAsync(id);

            product.PutOffSale(input.Reason);

            await _productRepo.UpdateAsync(product);

            return Mapper.Map<ProductDto>(product);
        }

        /// <summary>
        /// 商品分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PageModelDto<ProductDto>> GetPagedAsync(ProductSearchPagedDto search)
        {
            Expression<Func<Product, bool>> whereCondition = x => true;
            if (!search.Id.IsNullOrEmpty())
            {
                whereCondition = whereCondition.And(x => x.Id == search.Id.ToLong());
            }
            var pagedEntity = await _productRepo.PagedAsync(search.PageIndex, search.PageSize, whereCondition, x => x.Id);

            var pagedDto = Mapper.Map<PageModelDto<ProductDto>>(pagedEntity);

            if (pagedDto.Data.Count > 0)
            {
                //调用maint微服务获取字典,组合商品状态信息
                var rpcReuslt = await _maintRpcSrv.GetDictAsync(DictConsts.ProdunctStatusId);
                if (rpcReuslt.IsSuccessStatusCode && rpcReuslt.Content.Children.Count > 0)
                {
                    var dicts = rpcReuslt.Content.Children;
                    pagedDto.Data.ForEach(x =>
                    {
                        x.StatusDescription = dicts.FirstOrDefault(d => d.Name == x.StatusCode.ToString())?.Value;
                    });
                }
            }
            return pagedDto;
        }


        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<ProductDto>> GetListAsync(ProductSearchListDto search)
        {
            var whereCondition = ExpressionExtension.True<Product>();
            if (!search.Ids.IsNullOrEmpty())
            {
                var ids = search.Ids.Where(x => x.IsNotNullOrEmpty()).Select(x => x.ToLong().Value).Distinct();

                whereCondition = whereCondition.And(x => ids.Contains(x.Id));
            }
            if (search.StatusCode>0)
            {
                whereCondition = whereCondition.And(x => (int)x.Status.Code == search.StatusCode);
            }

            var products = await _productRepo.Where(whereCondition).ToListAsync();

            var productsDto = Mapper.Map<List<ProductDto>>(products);

            return productsDto;
        }
    }
}
