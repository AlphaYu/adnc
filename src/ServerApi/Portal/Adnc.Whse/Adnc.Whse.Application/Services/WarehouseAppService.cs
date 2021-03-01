using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Extensions;
using Adnc.Application.Shared.Dtos;
using Adnc.Core.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Whse.Domain.Entities;
using Adnc.Whse.Domain.Services;
using Adnc.Whse.Application.Dtos;

namespace Adnc.Whse.Application.Services
{
    public class WarehouseAppService : AppService, IWarehouseAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfBasicRepository<Warehouse> _warehouseRepo;
        private readonly IEfBasicRepository<Product> _productRepo;
        private readonly WarehouseManager _warehouseManager;

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

        public async Task<WarehouseDto> CreateAsync(WarehouseCreationDto input)
        {
            var warehouse = await _warehouseManager.CreateAsync(input.PositionCode, input.PositionDescription);

            await _warehouseRepo.InsertAsync(warehouse);

            return _mapper.Map<WarehouseDto>(warehouse);
        }

        [UnitOfWork(SharedToCap = true)]
        public async Task<WarehouseDto> AllocateShelfToProductAsync(long warehouseId, WarehouseAllocateToProductDto input)
        {
            var warehouse = await _warehouseRepo.GetAsync(warehouseId);
            var product = await _productRepo.GetAsync(input.ProductId.ToLong().Value);

            await _warehouseManager.AllocateShelfToProductAsync(warehouse, product);

            await _warehouseRepo.UpdateAsync(warehouse);

            return _mapper.Map<WarehouseDto>(warehouse);
        }

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
