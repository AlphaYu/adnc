using Adnc.Application.Shared.Services;
using Adnc.Warehouse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Warehouse.Core.Services;
using Adnc.Warehouse.Application.Dtos;
using Adnc.Core.Shared.IRepositories;
using AutoMapper;
using Adnc.Infr.Common.Extensions;

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
            return _mapper.Map<ShelfDto>(shelf);
        }

        public async Task<ShelfDto> AllocateShelfToProductAsync(long shelfId,ShelfAllocateToProductDto input)
        {
            var shelf = await _shelfRepo.FindAsync(shelfId);
            var product = await _productRepo.FindAsync(input.ProductId.ToLong().Value);

            await _shelfManager.AllocateShelfToProductAsync(shelf, product);

            await _shelfRepo.UpdateAsync(shelf);

            return _mapper.Map<ShelfDto>(shelf);
        }
    }
}
