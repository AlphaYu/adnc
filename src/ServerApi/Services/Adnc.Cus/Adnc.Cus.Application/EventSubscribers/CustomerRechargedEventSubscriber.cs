using System.Threading.Tasks;
using DotNetCore.CAP;
using Adnc.Infra.EventBus.Cap;
using Adnc.Cus.Core.Services;

namespace Adnc.Cus.Application.EventSubscribers
{
    /// <summary>
    /// 充值事件订阅者
    /// </summary>
    public class CustomerRechargedEventSubscriber : CapSubscriber
    {
        private CustomerManagerService _customerMgr;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customerMgr"></param>
        public CustomerRechargedEventSubscriber(
            CustomerManagerService customerMgr)
        {
            _customerMgr = customerMgr;
        }

        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="eto"></param>
        /// <returns></returns>
        [CapSubscribe(nameof(Core.Events.CustomerRechargedEvent))]
        public async Task Process(Core.Events.CustomerRechargedEvent eto)
        {
            await _customerMgr.ProcessRechargingAsync(eto.Data.TransactionLogId, eto.Data.CustomerId, eto.Data.Amount);
        }
    }
}
