using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Adnc.Infr.Common.Exceptions;
using Adnc.Infr.Common.Helper;
using Adnc.Infr.Common.Extensions;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.RpcServices;
using Adnc.Core.Shared.IRepositories;
using Adnc.Core.Shared.Domain.Entities;
using Adnc.Orders.Application.Dtos;
using Adnc.Orders.Domain.Services;
using Adnc.Orders.Domain.Entities;
using Adnc.Orders.Application.RpcServices;

namespace Adnc.Orders.Application.Services
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderAppService : AppService, IOrderAppService
    {
        private readonly OrderManager _orderMgr;
        private readonly IEfRepository<Order> _orderRepo;
        private readonly IWarehouseRpcService _warehouseRpc;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderRepo"></param>
        /// <param name="orderMgr"></param>
        /// <param name="warehouseRpc"></param>
        /// <param name="mapper"></param>
        public OrderAppService(
             IEfRepository<Order> orderRepo
            , OrderManager orderMgr
            , IWarehouseRpcService warehouseRpc
            , IMapper mapper)
        {
            _orderRepo = orderRepo;
            _orderMgr = orderMgr;
            _warehouseRpc = warehouseRpc;
            _mapper = mapper;
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
            var rpcResult = await _warehouseRpc.GetListAsync(productIds);
            if (!rpcResult.IsSuccessStatusCode)
                throw new BusinessException(rpcResult.StatusCode, "");

            var orderId = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);

            var items = from o in input.Items
                        join p in rpcResult.Content on o.ProductId equals p.Id
                        select (new OrderItemProduct(p.Id.ToLong().Value, p.Name, p.Price), o.Count);

            var order = await _orderMgr.CreateAsync(orderId
                ,
                input.CustomerId
                ,
                items
                ,
                new OrderDeliveryInfomation(input.DeliveryInfomaton?.Name, input.DeliveryInfomaton?.Phone, input.DeliveryInfomaton?.Address)
                );

            return _mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OrderDto> UpdateAsync(long id, OrderUpdationDto input)
        {
            var order = await _orderRepo.FindAsync(id, noTracking: false);
            order.CheckIsNormal();

            order.ChangeDeliveryInfomation(new OrderDeliveryInfomation(
                input.DeliveryInfomaton.Name
                , input.DeliveryInfomaton.Phone
                , input.DeliveryInfomaton.Address)
            );

            await _orderRepo.UpdateAsync(order);

            return _mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            var order = await _orderRepo.FindAsync(id);
            order.CheckIsNormal();

            order.ChangeStatus(OrderStatusEnum.Deleted);
            await _orderRepo.UpdateAsync(order);
        }

        /// <summary>
        /// 订单分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<PageModelDto<OrderDto>> GetPagedAsync(OrderSearchDto search)
        {
            Expression<Func<Order, bool>> whereCondition = x => true;
            if (search.Id > 0)
            {
                whereCondition = whereCondition.And(x => x.Id == search.Id);
            }
            var pagedEntity = _orderRepo.PagedAsync(search.PageIndex, search.PageSize, whereCondition, x => x.Id);

            var pagedDto = _mapper.Map<PageModelDto<OrderDto>>(pagedEntity);

            if (pagedDto.Data.Count > 0)
            {
                //调用maint微服务获取字典,组合商品状态信息
                //var rpcReuslt = await _maintRpcSrv.GetDictAsync(10000);
                //if (rpcReuslt.IsSuccessStatusCode && rpcReuslt.Content.Children.Count > 0)
                //{
                //    var dicts = rpcReuslt.Content.Children;
                //    pagedDto.Data.ForEach(x =>
                //    {
                //        //x.Status.StatusDescription = dicts.FirstOrDefault(d => d.Name == x.Status.StatusCode.ToSafeString())?.Name;
                //    });
                //}
            }
            return pagedDto;
        }

        /// <summary>
        /// 订单付款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDto> PayAsync(long id)
        {
            var order = await _orderRepo.FindAsync(id);
            order.CheckIsNormal();

            await _orderMgr.PayAsync(order);

            return _mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDto> CancelAsync(long id)
        {
            var order = await _orderRepo.FindAsync(id);
            order.CheckIsNormal();

            await _orderMgr.CancelAsync(order);

            return _mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDto> GetAsync(long id)
        {
            var order = await _orderRepo.FindAsync(id);
            order.CheckIsNormal();

            return _mapper.Map<OrderDto>(order);
        }
    }
}
