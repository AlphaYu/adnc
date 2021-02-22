using System.Threading.Tasks;
using DotNetCore.CAP;
using Adnc.Core.Shared.IRepositories;
using Adnc.Cus.Core.Entities;
using Adnc.Core.Shared;
using Adnc.Infr.EventBus;
using Adnc.Cus.Core.Events;

namespace Adnc.Cus.Core.Eventss.Subscribers
{
    public class CustomerRechargedSubscriber : CapSubscriber
    {
        private readonly IUnitOfWork _uow;
        private readonly IEfRepository<CusFinance> _cusFinanceReop;
        private readonly IEfRepository<CusTransactionLog> _cusTranlog;
        public CustomerRechargedSubscriber(IUnitOfWork uow
            , IEfRepository<CusFinance> cusFinanceReop
            , IEfRepository<CusTransactionLog> cusTranlog)
        {
            _uow = uow;
            _cusFinanceReop = cusFinanceReop;
            _cusTranlog = cusTranlog;
        }


        [CapSubscribe(nameof(CustomerRechargedEvent))]
        public async Task Process(CustomerRechargedEvent eto)
        {
            using (var trans = _uow.GetDbContextTransaction())
            {
                var transLog = await _cusTranlog.FindAsync(eto.Data.TransactionLogId);
                if (transLog == null || transLog.ExchageStatus == "20")
                    return;

                var finance = await _cusFinanceReop.FindAsync(eto.Data.CustomerId);
                var newBalance = finance.Balance + eto.Data.Amount;

                transLog.ExchageStatus = "20";
                transLog.ChangingAmount = finance.Balance;
                transLog.ChangedAmount = newBalance;


                await _cusFinanceReop.UpdateAsync(new CusFinance() { Id = finance.Id, Balance = newBalance }, UpdatingProps<CusFinance>(t => t.Balance));
                await _cusTranlog.UpdateAsync(transLog, UpdatingProps<CusTransactionLog>(t => t.ExchageStatus, t => t.ChangingAmount, t => t.ChangedAmount));

                _uow.Commit();
            }
        }
    }
}
