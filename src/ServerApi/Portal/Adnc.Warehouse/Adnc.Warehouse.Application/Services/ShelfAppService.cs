using Adnc.Application.Shared.Services;
using Adnc.Warehouse.Core.Entities;
using System.Threading.Tasks;
using Adnc.Warehouse.Core.Services;
using Adnc.Warehouse.Application.Dtos;
using Adnc.Core.Shared.IRepositories;
using AutoMapper;
using Adnc.Infr.Common.Extensions;
using Adnc.Application.Shared.Dtos;
using System.Linq;

namespace Adnc.Warehouse.Application.Services
{
    public class ShelfAppService : AppService, IShelfAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<Shelf> _shelfRepo;
        private readonly IEfRepository<Product> _productRepo;
        private readonly ShelfManager _shelfManager;

        public ShelfAppService(ShelfManager shelfManager
            , IMapper mapper
            , IEfRepository<Shelf> shelfRepo
            , IEfRepository<Product> productRepo)
        {
            _shelfManager = shelfManager;
            _shelfRepo = shelfRepo;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<ShelfDto> CreateAsync(ShelfCreationDto input)
        {
            var shelf = await _shelfManager.CreateAsync(input.PositionCode, input.PositionDescription);

            await _shelfRepo.InsertAsync(shelf);

            return _mapper.Map<ShelfDto>(shelf);
        }

        public async Task<ShelfDto> AllocateShelfToProductAsync(long shelfId, ShelfAllocateToProductDto input)
        {
            await Test();

            var shelf = await _shelfRepo.FindAsync(shelfId);
            var product = await _productRepo.FindAsync(input.ProductId.ToLong().Value);

            await _shelfManager.AllocateShelfToProductAsync(shelf, product);

            await _shelfRepo.UpdateAsync(shelf);

            return _mapper.Map<ShelfDto>(shelf);
        }

        public async Task<PageModelDto<ShelfDto>> GetPagedAsync(ShlefSearchDto search)
        {
            var total = await _shelfRepo.CountAsync(x => true);

            if (total == 0)
                return new PageModelDto<ShelfDto>
                {
                    TotalCount = 0
                    ,
                    PageIndex = search.PageIndex
                    ,
                    PageSize = search.PageSize
                    ,
                    PageCount = 0
                };

            var products = _productRepo.GetAll();
            var shelfs = _shelfRepo.GetAll();

            var skipNumber = (search.PageIndex - 1) * search.PageSize;

            var data = await (from s in shelfs
                              join p in products
                              on s.Id equals p.ShlefId into sp
                              from x in sp.DefaultIfEmpty()
                              select new ShelfDto()
                              {
                                  Id = s.Id.ToString()
                                  ,
                                  FreezedQty = s.FreezedQty
                                  //,
                                  //Position = _mapper.Map<ShelfPositionDto>(s.Position)
                                  ,
                                  ProductId = s.ToSafeString()
                                  ,
                                  ProductName = x.Name
                                  ,
                                  ProductSku = x.Name
                                  ,
                                  Qty = s.Qty
                              })
                           .Skip(skipNumber)
                           .Take(search.PageSize)
                           .OrderByDescending(x => x.Id)
                           .ToListAsync();

            return new PageModelDto<ShelfDto>()
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

        public async Task Test()
        {
            await Task.Delay(1000);
            await _shelfRepo.CountAsync(x=>true);
        }
    }
}
