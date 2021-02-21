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
        /// 订单创建
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerId"></param>
        /// <param name="products"></param>
        /// <param name="deliveryInfomation"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public virtual async Task<Order> CreateAsync(long id, long customerId
            , IEnumerable<(OrderItemProduct Product, int Count)> items
            , OrderDeliveryInfomation deliveryInfomation = null
            , string remark = null)
        {
            var order = new Order(
                id
               , customerId
               , deliveryInfomation
               , remark
            );

            //AddProduct会判断是否有重复的产品
            foreach (var item in items)
            {
                order.AddProduct(new OrderItemProduct(item.Product.Id, item.Product.Name, item.Product.Price), item.Count);
            }

            await _orderRepo.InsertAsync(order);

            //发送OrderCreatedEvent事件，通知仓储中心冻结库存
            var products = order.Items.Select(x => (x.Id, x.Count)).ToArray();

            var eventId = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);
            var eventData = new OrderCreatedEvent.EventData() { OrderId = order.Id, Products = products };
            await _eventPubliser.PublishAsync(new OrderCreatedEvent(eventId, eventData));
            return order;
        }


        /// <summary>
        /// 订单取消，没有付款的订单可以取消
        /// </summary>
        /// <returns></returns>
        public virtual async Task CancelAsync(Order order)
        {
            order.ChangeStatus(OrderStatusEnum.Canceling);
            await _orderRepo.UpdateAsync(order);

            //发布领域事件，通知仓储中心解冻被冻结的库存
            var eventId = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);
            var eventData = new OrderCanceledEvent.EventData() { OrderId = order.Id };
            await _eventPubliser.PublishAsync(new OrderCanceledEvent(eventId, eventData));
        }

        /// <summary>
        /// 订单申请付款
        /// </summary>
        /// <returns></returns>
        public virtual async Task RequestPaymentAsync(Order order)
        {
            order.ChangeStatus(OrderStatusEnum.Paying);
            await _orderRepo.UpdateAsync(order);

            //发布领域事件，通知客户中心扣款(Demo是从余额中扣款)
            var eventId = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);
            var eventData = new OrderRequestedPaymentEvent.EventData() { OrderId = order.Id, Amount = order.Amount };
            await _eventPubliser.PublishAsync(new OrderRequestedPaymentEvent(eventId, eventData));
        }
    }
}
