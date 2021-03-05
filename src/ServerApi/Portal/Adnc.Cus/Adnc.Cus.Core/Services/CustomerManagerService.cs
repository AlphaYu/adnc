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
        private readonly IEfRepository<CustomerFinance> _cusFinaceRepo;
        private readonly IEfRepository<CustomerTransactionLog> _cusTransactionLogRepo;
        private readonly IEventPublisher _eventPublisher;

        public CustomerManagerService(IEfRepository<Customer> cusRepo
            , IEfRepository<CustomerFinance> cusFinaceRepo
            , IEfRepository<CustomerTransactionLog> cusTransactionLogRepo
            , IEventPublisher eventPublisher)
        {
            _cusRepo = cusRepo;
            _cusFinaceRepo = cusFinaceRepo;
            _cusTransactionLogRepo = cusTransactionLogRepo;
            _eventPublisher = eventPublisher;
        }

        public CustomerManagerService(IEfRepository<Customer> cusRepo
            , IEfRepository<CustomerFinance> cusFinaceRepo
            , IEfRepository<CustomerTransactionLog> cusTransactionLogRepo)
        {
            _cusRepo = cusRepo;
            _cusFinaceRepo = cusFinaceRepo;
            _cusTransactionLogRepo = cusTransactionLogRepo;
        }

        [UnitOfWork]
        public virtual async Task RegisterAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            await _cusRepo.InsertAsync(customer);
        }

        [UnitOfWork(SharedToCap = true)]
        public virtual async Task RechargeAsync(CustomerTransactionLog cusTransactionLog, CancellationToken cancellationToken = default)
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
            var transLog = await _cusTransactionLogRepo.FindAsync(transactionLogId, noTracking: false);
            if (transLog == null || transLog.ExchageStatus != ExchageStatusEnum.Processing)
                return;

            var finance = await _cusFinaceRepo.FindAsync(customerId, noTracking: false);
            var originalBalance = finance.Balance;
            var newBalance = originalBalance + amount;

            finance.Balance = newBalance;
            await _cusFinaceRepo.UpdateAsync(finance);

            transLog.ExchageStatus = ExchageStatusEnum.Finished;
            transLog.ChangingAmount = originalBalance;
            transLog.ChangedAmount = newBalance;
            await _cusTransactionLogRepo.UpdateAsync(transLog);
        }

        [UnitOfWork(SharedToCap = true)]
        public virtual async Task ProcessPayingAsync(long transactionLogId, long customerId, decimal amount)
        {
            bool paidResult = false;

            var transLog = await _cusTransactionLogRepo.FindAsync(transactionLogId);
            if (transLog != null)
                return;

            var account = await _cusRepo.FetchAsync(x => x.Account, x => x.Id == customerId);

            var finance = await _cusFinaceRepo.FindAsync(customerId, noTracking: false);
            var originalBalance = finance.Balance;
            var newBalance = originalBalance - amount;

            if (newBalance >= 0)
            {
                finance.Balance = newBalance;
                await _cusFinaceRepo.UpdateAsync(finance);

                transLog = new CustomerTransactionLog
                {
                    Id = transactionLogId
                    ,
                    CustomerId = customerId
                    ,
                    Account = account
                    ,
                    ChangingAmount = originalBalance
                    ,
                    Amount = 0 - amount
                    ,
                    ChangedAmount = newBalance
                    ,
                    ExchangeType = ExchangeTypeEnum.Order
                    ,
                    ExchageStatus = ExchageStatusEnum.Finished

                };

                await _cusTransactionLogRepo.InsertAsync(transLog);

                paidResult = true;
            }

            //_eventPublisher.PublishAsync()
        }
    }
}
