using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Adnc.Core.Shared.IRepositories;
using Adnc.Cus.Core.Entities;
using Adnc.Core.Shared;
using Adnc.Core.Shared.EventBus;
using Adnc.Cus.Core.EventBus.Etos;

namespace Adnc.Cus.Core.EventBus.Subscribers
{
    public interface ICustomerRechargedSubscriber
    {
        Task Process(CustomerRechargedEto eto);
    }

    public class CustomerRechargedSubscriber : ICustomerRechargedSubscriber, ICapSubscribe
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


        [CapSubscribe(EbConsts.CustomerRechagered)]
        public async Task Process(CustomerRechargedEto eto)
        {
            try
            {
                _uow.BeginTransaction();

                var transLog = await _cusTranlog.FindAsync(eto.TransactionLogId);
                if (transLog == null || transLog.ExchageStatus == "20")
                    return;

                var finance = await _cusFinanceReop.FindAsync(eto.Id);
                var newBalance = finance.Balance + eto.Amount;

                transLog.ExchageStatus = "20";
                transLog.ChangingAmount = finance.Balance;
                transLog.ChangedAmount = newBalance;


                await _cusFinanceReop.UpdateAsync(new CusFinance() { Id = finance.Id, Balance = newBalance }, t => t.Balance);
                await _cusTranlog.UpdateAsync(transLog, t => t.ExchageStatus, t => t.ChangingAmount, t => t.ChangedAmount);

                _uow.Commit();
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                _uow.Dispose();
            }
        }
    }
}
