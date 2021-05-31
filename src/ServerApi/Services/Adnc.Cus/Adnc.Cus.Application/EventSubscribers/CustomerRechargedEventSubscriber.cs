using Adnc.Cus.Application.Contracts.Services;
using Adnc.Infra.EventBus.Cap;
using DotNetCore.CAP;
using System.Threading.Tasks;

namespace Adnc.Cus.Application.EventSubscribers
{
    /// <summary>
    /// 充值事件订阅者
    /// </summary>
    public class CustomerRechargedEventSubscriber : CapSubscriber
    {
        private readonly ICustomerAppService _customerAppSrv;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customerMgr"></param>
        public CustomerRechargedEventSubscriber(ICustomerAppService customerAppSrv)
        {
            _customerAppSrv = customerAppSrv;
        }

        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="eto"></param>
        /// <returns></returns>
        [CapSubscribe(nameof(Core.Events.CustomerRechargedEvent))]
        public async Task Process(Core.Events.CustomerRechargedEvent eto)
        {
            await _customerAppSrv.ProcessRechargingAsync(eto.Data.TransactionLogId, eto.Data.CustomerId, eto.Data.Amount);
        }
    }
}