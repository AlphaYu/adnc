using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Adnc.Core.Shared.IRepositories;
using Adnc.Cus.Core.Entities;
using Adnc.Core.Shared;

namespace Adnc.Cus.Core.EventBus
{
    public interface IRechargeSubscriber
    {
        Task Process(RechargeEbModel rechargeEbModel);
    }

    public class RechargeSubscriber : IRechargeSubscriber, ICapSubscribe
    {
        private readonly IUnitOfWork _uow;
        private readonly IEfRepository<CusFinance> _cusFinanceReop;
        private readonly IEfRepository<CusTransactionLog> _cusTranlog;
        public RechargeSubscriber(IUnitOfWork uow
            , IEfRepository<CusFinance> cusFinanceReop
            , IEfRepository<CusTransactionLog> cusTranlog)
        {
            _uow = uow;
            _cusFinanceReop = cusFinanceReop;
            _cusTranlog = cusTranlog;
        }

        [CapSubscribe(EbConsts.CustomerRechager)]
        public async Task Process(RechargeEbModel rechargeEbModel)
        {
            var transLog = await _cusTranlog.FindAsync(rechargeEbModel.TransactionLogId);
            if (transLog == null || transLog.ExchageStatus == "20")
                return;

            try
            {
                var finance = await _cusFinanceReop.FindAsync(rechargeEbModel.ID);
                var newBalance = finance.Balance + rechargeEbModel.Amount;

                transLog.ExchageStatus = "20";
                transLog.ChangingAmount = finance.Balance;
                transLog.ChangedAmount = newBalance;

                _uow.BeginTransaction();

                await _cusFinanceReop.UpdateAsync(new CusFinance() { ID = finance.ID, Balance = newBalance }, t => t.Balance);
                await _cusTranlog.UpdateAsync(transLog, t => t.ExchageStatus, t => t.ChangingAmount, t => t.ChangedAmount);

                _uow.Commit();
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                throw new Exception(ex.Message,ex);
            }
            finally
            {
                _uow.Dispose();
            }
        }
    }
}
