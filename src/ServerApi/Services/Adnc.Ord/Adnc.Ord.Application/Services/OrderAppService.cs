using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Core;
using Adnc.Infra.Helper;
using Adnc.Infra.IRepositories;
using Adnc.Ord.Application.Contracts.Dtos;
using Adnc.Ord.Application.Contracts.Services;
using Adnc.Ord.Core.Services;
using Adnc.Ord.Domain.Entities;
using Adnc.Shared.RpcServices;
using Adnc.Shared.RpcServices.Rtos;
using Adnc.Shared.RpcServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Adnc.Ord.Application.Services
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderAppService : AbstractAppService, IOrderAppService
    {
        private readonly OrderManager _orderMgr;
        private readonly IEfBasicRepository<Order> _orderRepo;
        private readonly IWhseRpcService _warehouseRpc;
        private readonly IMaintRpcService _maintRpc;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderRepo"></param>
        /// <param name="orderMgr"></param>
        /// <param name="warehouseRpc"></param>
        /// <param name="maintRpc"></param>
        /// <param name="mapper"></param>
        public OrderAppService(
             IEfBasicRepository<Order> orderRepo
            , OrderManager orderMgr
            , IWhseRpcService warehouseRpc
            , IMaintRpcService maintRpc)
        {
            _orderRepo = orderRepo;
            _orderMgr = orderMgr;
            _warehouseRpc = warehouseRpc;
            _maintRpc = maintRpc;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OrderDto> CreateAsync(OrderCreationDto input)
        {
            var productIds = input.Items.Select(x => x.ProductId).ToArray();

            //获取产品的价格,名字
            var rpcResult = await _warehouseRpc.GetProductsAsync(new ProductSearchListRto { Ids = productIds });
            if (!rpcResult.IsSuccessStatusCode)
            {
                var apiError = ((Refit.ValidationApiException)rpcResult.Error).Content;
                throw new BusinessException(rpcResult.StatusCode, apiError.Detail);
            }

            var orderId = IdGenerater.GetNextId();
            var products = rpcResult.Content;

            var items = from o in input.Items
                        join p in products on o.ProductId equals p.Id
                        select (new OrderItemProduct(p.Id, p.Name, p.Price), o.Count);

            //需要发布领域事件,订单中心订阅该事件
            var order = await _orderMgr.CreateAsync(orderId
                ,
                input.CustomerId
                ,
                items
                ,
                new OrderReceiver(input.DeliveryInfomaton?.Name, input.DeliveryInfomaton?.Phone, input.DeliveryInfomaton?.Address)
                );

            await _orderRepo.InsertAsync(order);

            return Mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// 标记订单创建状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task MarkCreatedStatusAsync(long id, OrderMarkCreatedStatusDto input)
        {
            var order = await _orderRepo.GetAsync(id);
            Checker.NotNull(order, nameof(order));

            //需要发布领域事件，仓储中心订阅该事件
            order.MarkCreatedStatus(input.IsSuccess, input.Remark);

            await _orderRepo.UpdateAsync(order);
        }

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OrderDto> UpdateAsync(long id, OrderUpdationDto input)
        {
            var order = await _orderRepo.GetAsync(id);

            Checker.NotNull(order, nameof(order));

            order.ChangeReceiver(new OrderReceiver(
                input.DeliveryInfomaton.Name
                , input.DeliveryInfomaton.Phone
                , input.DeliveryInfomaton.Address)
            );

            await _orderRepo.UpdateAsync(order);

            return Mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            var order = await _orderRepo.GetAsync(id);

            Checker.NotNull(order, nameof(order));

            order.MarkDeletedStatus("");

            await _orderRepo.UpdateAsync(order);
        }

        /// <summary>
        /// 订单付款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDto> PayAsync(long id)
        {
            var order = await _orderRepo.GetAsync(id);

            //需要发布领域事件，客户中心订阅该事件
            await _orderMgr.PayAsync(order);

            await _orderRepo.UpdateAsync(order);

            return Mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDto> CancelAsync(long id)
        {
            var order = await _orderRepo.GetAsync(id);

            //需要发布领域事件，仓储中心订阅该事件
            await _orderMgr.CancelAsync(order);

            await _orderRepo.UpdateAsync(order);

            return Mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDto> GetAsync(long id)
        {
            var order = await _orderRepo.GetAsync(id, x => x.Items);
            return Mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// 订单分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PageModelDto<OrderDto>> GetPagedAsync(OrderSearchPagedDto search)
        {
            var whereCondition = ExpressionCreator
                                                                 .New<Order>()
                                                                 .AndIf(search.Id > 0, x => x.Id == search.Id);

            var pagedEntity = await _orderRepo.PagedAsync(search.PageIndex, search.PageSize, whereCondition, x => x.Id);

            var pagedDto = Mapper.Map<PageModelDto<OrderDto>>(pagedEntity);

            if (pagedDto.Data.Count > 0)
            {
                //调用maint微服务获取字典,组合订单状态信息
                var rpcReuslt = await _maintRpc.GetDictAsync(Consts.OrderStatusId);
                if (rpcReuslt.IsSuccessStatusCode && rpcReuslt.Content.Children.Count > 0)
                {
                    var dicts = rpcReuslt.Content.Children;
                    pagedDto.Data.ForEach(x =>
                    {
                        x.StatusChangesReason = dicts.FirstOrDefault(d => d.Name == x.StatusCode.ToSafeString())?.Name;
                    });
                }
            }
            return pagedDto;
        }
    }
}