using Adnc.Application.Shared.Services;
using Adnc.Whse.Domain.Entities;
using System.Threading.Tasks;
using Adnc.Whse.Domain.Services;
using Adnc.Whse.Application.Dtos;
using Adnc.Core.Shared.IRepositories;
using AutoMapper;
using Adnc.Infr.Common.Extensions;
using Adnc.Application.Shared.Dtos;
using System.Linq;
using Adnc.Core.Shared.Interceptors;

namespace Adnc.Whse.Application.Services
{
    public class WarehouseAppService : AppService, IWarehouseAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<Warehouse> _shelfRepo;
        private readonly IEfRepository<Product> _productRepo;
        private readonly WarehouseManager _warehouseManager;

        public WarehouseAppService(WarehouseManager warehouseManager
            , IMapper mapper
            , IEfRepository<Warehouse> shelfRepo
            , IEfRepository<Product> productRepo)
        {
            _warehouseManager = warehouseManager;
            _shelfRepo = shelfRepo;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<WarehouseDto> CreateAsync(WarehouseCreationDto input)
        {
            var shelf = await _warehouseManager.CreateAsync(input.PositionCode, input.PositionDescription);

            await _shelfRepo.InsertAsync(shelf);

            return _mapper.Map<WarehouseDto>(shelf);
        }

        [UnitOfWork(SharedToCap = true)]
        public async Task<WarehouseDto> AllocateShelfToProductAsync(long shelfId, WarehouseAllocateToProductDto input)
        {
            var shelf = await _shelfRepo.FindAsync(shelfId, noTracking: false);
            var product = await _productRepo.FindAsync(input.ProductId.ToLong().Value);

            await _warehouseManager.AllocateShelfToProductAsync(shelf, product);

            await _shelfRepo.UpdateAsync(shelf);

            return _mapper.Map<WarehouseDto>(shelf);
        }

        public async Task<PageModelDto<WarehouseDto>> GetPagedAsync(WarehouseSearchDto search)
        {
            var total = await _shelfRepo.CountAsync(x => true);

            if (total == 0)
                return new PageModelDto<WarehouseDto>
                {
                    TotalCount = 0
                    ,
                    PageIndex = search.PageIndex
                    ,
                    PageSize = search.PageSize
                };

            var products = _productRepo.GetAll();
            var shelfs = _shelfRepo.GetAll();

            var skipNumber = (search.PageIndex - 1) * search.PageSize;

            var data = await (from s in shelfs
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
    }
}
