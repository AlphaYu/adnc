using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Adnc.Cus.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Core.Shared;
using Adnc.Core.Shared.Interceptors;
using Adnc.Cus.Core.EventBus.Etos;

namespace Adnc.Cus.Core.Services
{
    public class CustomerManagerService : ICoreService
    {
        private readonly IEfRepository<Customer> _cusRepo;
        private readonly IEfRepository<CusFinance> _cusFinaceRepo;
        private readonly IEfRepository<CusTransactionLog> _cusTransactionLogRepo;
        private readonly ICapPublisher _capBus;

        public CustomerManagerService(IEfRepository<Customer> cusRepo
            , IEfRepository<CusFinance> cusFinaceRepo
            , IEfRepository<CusTransactionLog> cusTransactionLogRepo
            , ICapPublisher capBus)
        {
            _cusRepo = cusRepo;
            _cusFinaceRepo = cusFinaceRepo;
            _cusTransactionLogRepo = cusTransactionLogRepo;
            _capBus = capBus;
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
        public virtual async Task Register(Customer customer, CusFinance cusFinance, CancellationToken cancellationToken = default)
        {
            await _cusRepo.InsertAsync(customer);
            await _cusFinaceRepo.InsertAsync(cusFinance);
        }

        [UnitOfWork(SharedToCap =true)]
        public virtual async Task Recharge(long customerId, decimal amount, CusTransactionLog cusTransactionLog, CancellationToken cancellationToken = default)
        {

            await _cusTransactionLogRepo.InsertAsync(cusTransactionLog);

            var regchargeInfo = new CustomerRechargedEto
            {
                Id = customerId
                ,
                Amount = amount
                ,
                TransactionLogId = cusTransactionLog.Id
                ,
                EventSource = nameof(this.Recharge)
            };

            await _capBus.PublishAsync(EbConsts.CustomerRechagered, regchargeInfo);
        }
    }
}
