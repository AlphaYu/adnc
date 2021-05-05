using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Adnc.Core.Shared;
using Adnc.Infra.EventBus;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Common.Helper;
using Adnc.Ord.Core.Entities;
using Adnc.Ord.Core.Events;
using Adnc.Infra.Common.Exceptions;

namespace Adnc.Ord.Core.Services
{
    /// <summary>
    /// 订单领域服务
    /// </summary>
    public class OrderManager : ICoreService
    {
        private readonly IEfBasicRepository<Order> _orderRepo;
        private readonly IEventPublisher _eventPubliser;

        public OrderManager(IEfBasicRepository<Order> orderRepo
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
            , OrderReceiver deliveryInfomation = null
            , string remark = null)
        {
            var order = new Order(
                id
               , customerId
               , deliveryInfomation
               , remark
            );

            //AddProduct会判断是否有重复的产品
            foreach (var (Product, Count) in items)
            {
                order.AddProduct(new OrderItemProduct(Product.Id, Product.Name, Product.Price), Count);
            }

            //发送OrderCreatedEvent事件，通知仓储中心冻结库存
            var products = order.Items.Select(x => (x.Id, x.Count)).ToArray();
            var eventId = IdGenerater.GetNextId();
            var eventData = new OrderCreatedEvent.EventData() { OrderId = order.Id, Products = products };
            var eventSource = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            await _eventPubliser.PublishAsync(new OrderCreatedEvent(eventId, eventData, eventSource));
            return order;
        }


        /// <summary>
        /// 订单取消，没有付款的订单可以取消
        /// </summary>
        /// <returns></returns>
        public virtual async Task CancelAsync(Order order)
        {
            Checker.NotNull(order, nameof(order));

            order.ChangeStatus(OrderStatusEnum.Canceling,string.Empty);

            //发布领域事件，通知仓储中心解冻被冻结的库存
            var eventId = IdGenerater.GetNextId();
            var eventData = new OrderCanceledEvent.EventData() { OrderId = order.Id };
            var eventSource = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            await _eventPubliser.PublishAsync(new OrderCanceledEvent(eventId, eventData, eventSource));
        }

        /// <summary>
        /// 订单付款
        /// </summary>
        /// <returns></returns>
        public virtual async Task PayAsync(Order order)
        {
            Checker.NotNull(order, nameof(order));

            order.ChangeStatus(OrderStatusEnum.Paying,string.Empty);

            //发布领域事件，通知客户中心扣款(Demo是从余额中扣款)
            var eventId = IdGenerater.GetNextId();
            var eventData = new OrderPaidEvent.EventData() { OrderId = order.Id, CustomerId = order.CustomerId, Amount = order.Amount };
            var eventSource = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            await _eventPubliser.PublishAsync(new OrderPaidEvent(eventId, eventData, eventSource));
        }
    }
}
