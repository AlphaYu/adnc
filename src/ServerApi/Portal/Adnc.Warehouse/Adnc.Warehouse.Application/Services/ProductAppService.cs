using System;
using System.Threading.Tasks;
using AutoMapper;
using Adnc.Warehouse.Application.Dtos;
using Adnc.Warehouse.Core.Services;
using Adnc.Warehouse.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using System.Linq;
using System.Linq.Expressions;
using Adnc.Infr.Common.Extensions;
using Adnc.Application.Shared.RpcServices;

namespace Adnc.Warehouse.Application.Services
{
    /// <summary>
    /// 商品管理
    /// </summary>
    public class ProductAppService : AppService, IProductAppService
    {
        private readonly ProductManager _productMgr;
        private readonly IEfRepository<Product> _productRepo;
        private readonly IEfRepository<Shelf> _warehouseInfoRepo;
        private readonly IMaintRpcService _maintRpcSrv;
        private readonly IMapper _mapper;

        /// <summary>
        /// 商品管理构造函数
        /// </summary>
        /// <param name="productRepo"></param>
        /// <param name="warehouseInfoRepo"></param>
        /// <param name="maintRpcSrv"></param>
        /// <param name="productMgr"></param>
        /// <param name="mapper"></param>
        public ProductAppService(
             IEfRepository<Product> productRepo
            , IEfRepository<Shelf> warehouseInfoRepo
            , IMaintRpcService maintRpcSrv
            , ProductManager productMgr
            , IMapper mapper)
        {
            _productMgr = productMgr;
            _productRepo = productRepo;
            _warehouseInfoRepo = warehouseInfoRepo;
            _maintRpcSrv = maintRpcSrv;
            _mapper = mapper;
        }

        /// <summary>
        /// 创建商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> CreateAsync(ProductCreationDto input)
        {
            var product = await _productMgr.CreateAsync(input.Sku, input.Price, input.Name,input.Unit, input.Describe);

            await _productRepo.InsertAsync(product);

            return _mapper.Map<ProductDto>(product);
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> UpdateAsync(long id, ProductUpdationDto input)
        {
            var product = await _productRepo.FindAsync(id);

            product.Describe = input.Describe;
            product.Unit = input.Unit;

            product.SetPrice(input.Price);


            await _productMgr.ChangeSkuAsync(product, input.Sku);
            await _productMgr.ChangeNameAsync(product, input.Name);

            await _productRepo.UpdateAsync(product);

            return _mapper.Map<ProductDto>(product);
        }

        /// <summary>
        /// 调整价格
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> ChangePriceAsync(long id, ProducChangePriceDto input)
        {
            var product = await _productRepo.FindAsync(id);

            product.SetPrice(input.Price);

            await _productRepo.UpdateAsync(product);

            return _mapper.Map<ProductDto>(product);
        }

        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> PutOnSaleAsync(long id,ProductPutOnSaleDto input)
        {
            var product = await _productRepo.FindAsync(id);

            var warehouseInfo = await _warehouseInfoRepo.FetchAsync(x => x, x => x.ProductId == id);

            await _productMgr.PutOnSale(product, warehouseInfo, input.Reason);

            await _productRepo.UpdateAsync(product);

            return _mapper.Map<ProductDto>(product);
        }

        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> PutOffSaleAsync(long id, ProductPutOffSaleDto input)
        {
            var product = await _productRepo.FindAsync(id);

            product.PutOffSale(input.Reason);

            await _productRepo.UpdateAsync(product);

            return _mapper.Map<ProductDto>(product);
        }

        /// <summary>
        /// 商品分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PageModelDto<ProductDto>> GetPagedAsync(ProductSearchDto search)
        {
            Expression<Func<Product, bool>> whereCondition = x => true;
            if (search.Id > 0)
            {
                whereCondition = whereCondition.And(x => x.Id == search.Id);
            }
            var pagedEntity = _productRepo.PagedAsync(search.PageIndex, search.PageSize, whereCondition, x => x.Id);

            var pagedDto = _mapper.Map<PageModelDto<ProductDto>>(pagedEntity);

            if (pagedDto.Data.Count > 0)
            {
                //调用maint微服务获取字典,组合商品状态信息
                var rpcReuslt = await _maintRpcSrv.GetDictAsync(10000);
                if (rpcReuslt.IsSuccessStatusCode && rpcReuslt.Content.Children.Count > 0)
                {
                    var dicts = rpcReuslt.Content.Children;
                    pagedDto.Data.ForEach(x =>
                    {
                        x.Status.StatusDescription = dicts.FirstOrDefault(d => d.Name == x.Status.StatusCode.ToSafeString())?.Name;
                    });
                }
            }
            return pagedDto;
        }
    }
}
