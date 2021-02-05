using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Core.Shared;
using Adnc.Core.Shared.IRepositories;
using Adnc.Warehouse.Core.Entities;
using Adnc.Warehouse.Core.EventBus.Etos;
using DotNetCore.CAP;

namespace Adnc.Warehouse.Core.EventBus.Subscriber
{
    public interface IShelfToProductAllocatedEventSubscirber
    {
        Task Process(ShelfToProductAllocatedEto eto);
    }

    public class ShelfToProductAllocatedEventSubscirber : IShelfToProductAllocatedEventSubscirber, ICapSubscribe
    {
        private readonly IUnitOfWork _uow;
        private readonly IEfRepository<Product> _productReop;

        public ShelfToProductAllocatedEventSubscirber(IUnitOfWork uow
            , IEfRepository<Product> productReop)
        {
            _uow = uow;
            _productReop = productReop;
        }

        [CapSubscribe(EventBusConsts.ShelfToProductAllocated)]
        public async Task Process(ShelfToProductAllocatedEto eto)
        {
            try
            {
                using (var trans = _uow.GetDbContextTransaction())
                {
                    var produdct = await _productReop.FindAsync(eto.ProductId);
                    if (produdct != null)
                    {
                        produdct.SetShelf(eto.ShelfId);
                        await _productReop.UpdateAsync(produdct);
                        trans.Commit();
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
