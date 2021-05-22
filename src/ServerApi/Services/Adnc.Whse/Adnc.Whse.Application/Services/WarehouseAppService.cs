using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Common.Extensions;
using Adnc.Whse.Application.Contracts.Dtos;
using Adnc.Whse.Application.Contracts.Services;
using Adnc.Whse.Core.Entities;
using Adnc.Whse.Core.Services;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;

namespace Adnc.Whse.Application.Services
{
    /// <summary>
    /// 仓储管理
    /// </summary>
    public class WarehouseAppService : AbstractAppService, IWarehouseAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfBasicRepository<Warehouse> _warehouseRepo;
        private readonly IEfBasicRepository<Product> _productRepo;
        private readonly WarehouseManager _warehouseManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="warehouseManager"></param>
        /// <param name="mapper"></param>
        /// <param name="warehouseRepo"></param>
        /// <param name="productRepo"></param>
        public WarehouseAppService(WarehouseManager warehouseManager
            , IMapper mapper
            , IEfBasicRepository<Warehouse> warehouseRepo
            , IEfBasicRepository<Product> productRepo)
        {
            _warehouseManager = warehouseManager;
            _warehouseRepo = warehouseRepo;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// 创建仓储
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WarehouseDto> CreateAsync(WarehouseCreationDto input)
        {
            var warehouse = await _warehouseManager.CreateAsync(input.PositionCode, input.PositionDescription);

            await _warehouseRepo.InsertAsync(warehouse);

            return _mapper.Map<WarehouseDto>(warehouse);
        }

        /// <summary>
        /// 分配仓储给商品
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WarehouseDto> AllocateShelfToProductAsync(long warehouseId, WarehouseAllocateToProductDto input)
        {
            var warehouse = await _warehouseRepo.GetAsync(warehouseId);
            var product = await _productRepo.GetAsync(input.ProductId.ToLong().Value);

            await _warehouseManager.AllocateShelfToProductAsync(warehouse, product);

            await _warehouseRepo.UpdateAsync(warehouse);

            return _mapper.Map<WarehouseDto>(warehouse);
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PageModelDto<WarehouseDto>> GetPagedAsync(WarehouseSearchDto search)
        {
            var total = await _warehouseRepo.CountAsync(x => true);

            if (total == 0)
                return new PageModelDto<WarehouseDto>
                {
                    TotalCount = 0
                    ,
                    PageIndex = search.PageIndex
                    ,
                    PageSize = search.PageSize
                };

            var products = _productRepo.Where(x => true);
            var warehouses = _warehouseRepo.Where(x => true);

            var skipNumber = (search.PageIndex - 1) * search.PageSize;

            var data = await (from s in warehouses
                              join p in products
                              on s.ProductId equals p.Id into sp
                              from x in sp.DefaultIfEmpty()
                              select new WarehouseDto()
                              {
                                  Id = s.Id.ToString()
                                  ,
                                  FreezedQty = s.BlockedQty
                                  ,
                                  PositionCode = s.Position.Code
                                  ,
                                  PositionDescription = s.Position.Description
                                  ,
                                  ProductId = s.ProductId.Value.ToString()
                                  ,
                                  ProductName = x.Name
                                  ,
                                  ProductSku = x.Sku
                                  ,
                                  Qty = s.Qty
                              })
                           .Skip(skipNumber)
                           .Take(search.PageSize)
                           .OrderByDescending(x => x.Id)
                           .ToListAsync();

            return new PageModelDto<WarehouseDto>()
            {
                PageIndex = search.PageIndex
                ,
                PageSize = search.PageSize
                ,
                TotalCount = total
                ,
                Data = data
            };
        }

        /// <summary>
        /// 锁定库存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BlockQtyAsync(WarehouseBlockQtyDto input)
        {
            var blockQtyProductsInfo = input.Products.ToDictionary(x => x.ProductId, x => x.Qty);
            var warehouses = await _warehouseRepo.Where(x => blockQtyProductsInfo.Keys.Contains(x.ProductId.Value), noTracking: false).ToListAsync();
            var products = await _productRepo.Where(x => blockQtyProductsInfo.Keys.Contains(x.Id)).ToListAsync();

            var result = await _warehouseManager.BlockQtyAsync(input.OrderId, blockQtyProductsInfo, warehouses, products);

            //库存都符合锁定条件才能批量更新数据库
            if (result)
                await _warehouseRepo.UpdateRangeAsync(warehouses);
        }
    }
}