using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Adnc.Core.Shared;
using Adnc.Core.Shared.Events;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Orders.Domain.Entities;
using Adnc.Orders.Domain.Events;
using Adnc.Orders.Domain.Events.Etos;

namespace Adnc.Orders.Domain.Services
{
    public class OrderManager : ICoreService
    {
        private readonly IEfRepository<Order> _orderRepo;
        private readonly IEventPublisher _eventPubliser;

        public OrderManager(IEfRepository<Order> orderRepo
            , IEventPublisher eventPublisher)
        {
            _orderRepo = orderRepo;
            _eventPubliser = eventPublisher;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="items"></param>
        /// <param name="deliveryInfomation"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public virtual async Task<Order> CreateAsync(long customerId
            , ICollection<OrderItem> items
            , OrderDeliveryInfomation deliveryInfomation = null
            , string remark = null)
        {
            var order = new Order(
               IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
               , customerId
               , items
               , deliveryInfomation
               , remark
            );

            await _orderRepo.InsertAsync(order);

            //发送OrderCreatedEvent事件，通知仓储中心冻结库存
            //仓储中心处理完该事件后，要求仓储中心发送事件EventConsts.OrderInventoryFreezedEvent
            var products = order.Items.Select(x => new OrderCreatedEventEto.Product { ProductId = x.Product.Id, Qty = x.Count }).ToArray();
            await _eventPubliser.PublishAsync(EventConsts.OrderCreatedEvent
                ,
                new OrderCreatedEventEto
                {
                    Id = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                    ,
                    OrderId = order.Id
                    ,
                    Products = products
                    ,
                    EventSource = nameof(OrderManager.CreateAsync)
                }
                ,
                EventConsts.OrderInventoryFreezedEvent
               );

            return order;
        }
    }
}
