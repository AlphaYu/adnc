using System.Threading;
using System.Threading.Tasks;
using Adnc.Cus.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Core.Shared;
using Adnc.Core.Shared.Interceptors;
using Adnc.Infr.EventBus;
using Adnc.Cus.Core.Events;
using Adnc.Infr.Common.Helper;

namespace Adnc.Cus.Core.Services
{
    public class CustomerManagerService : CoreService
    {
        private readonly IEfRepository<Customer> _cusRepo;
        private readonly IEfRepository<CusFinance> _cusFinaceRepo;
        private readonly IEfRepository<CusTransactionLog> _cusTransactionLogRepo;
        private readonly IEventPublisher _eventPublisher;

        public CustomerManagerService(IEfRepository<Customer> cusRepo
            , IEfRepository<CusFinance> cusFinaceRepo
            , IEfRepository<CusTransactionLog> cusTransactionLogRepo
            , IEventPublisher eventPublisher)
        {
            _cusRepo = cusRepo;
            _cusFinaceRepo = cusFinaceRepo;
            _cusTransactionLogRepo = cusTransactionLogRepo;
            _eventPublisher = eventPublisher;
        }

        public CustomerManagerService(IEfRepository<Customer> cusRepo
            , IEfRepository<CusFinance> cusFinaceRepo
            , IEfRepository<CusTransactionLog> cusTransactionLogRepo)
        {
            _cusRepo = cusRepo;
            _cusFinaceRepo = cusFinaceRepo;
            _cusTransactionLogRepo = cusTransactionLogRepo;
        }

        [UnitOfWork]
        public virtual async Task RegisterAsync(Customer customer, CusFinance cusFinance, CancellationToken cancellationToken = default)
        {
            await _cusRepo.InsertAsync(customer);
            await _cusFinaceRepo.InsertAsync(cusFinance);
        }

        [UnitOfWork(SharedToCap = true)]
        public virtual async Task RechargeAsync(CusTransactionLog cusTransactionLog, CancellationToken cancellationToken = default)
        {
            await _cusTransactionLogRepo.InsertAsync(cusTransactionLog);

            //发布充值事件
            var eventId = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);
            var eventData = new CustomerRechargedEvent.EventData() { CustomerId = cusTransactionLog.CustomerId, TransactionLogId = cusTransactionLog.Id, Amount = cusTransactionLog.Amount };
            var eventSource = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            await _eventPublisher.PublishAsync(new CustomerRechargedEvent(eventId, eventData, eventSource));
        }

        [UnitOfWork]
        public virtual async Task ProcessRechargingAsync(long transactionLogId, long customerId, decimal amount)
        {
            var transLog = await _cusTransactionLogRepo.FindAsync(transactionLogId);
            if (transLog == null || transLog.ExchageStatus == "20")
                return;

            var finance = await _cusFinaceRepo.FindAsync(customerId);
            var newBalance = finance.Balance + amount;

            transLog.ExchageStatus = "20";
            transLog.ChangingAmount = finance.Balance;
            transLog.ChangedAmount = newBalance;


            await _cusFinaceRepo.UpdateAsync(new CusFinance() { Id = finance.Id, Balance = newBalance }, UpdatingProps<CusFinance>(t => t.Balance));

            await _cusTransactionLogRepo.UpdateAsync(transLog, UpdatingProps<CusTransactionLog>(t => t.ExchageStatus, t => t.ChangingAmount, t => t.ChangedAmount));
        }
    }
}
