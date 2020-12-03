using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Adnc.Cus.Core.Entities;
using Adnc.Core.Shared.IRepositories;

namespace Adnc.Cus.Core.CoreServices
{
    public class CusManagerService : ICusManagerService
    {
        private readonly IEfRepository<Customer> _cusRepo;
        private readonly IEfRepository<CusFinance> _cusFinaceRepo;
        private readonly IEfRepository<CusTransactionLog> _cusTransactionLogRepo;
        private readonly ICapPublisher _capBus;

        public CusManagerService(IEfRepository<Customer> cusRepo
            , IEfRepository<CusFinance> cusFinaceRepo
            , IEfRepository<CusTransactionLog> cusTransactionLogRepo
            , ICapPublisher capBus)
        {
            _cusRepo = cusRepo;
            _cusFinaceRepo = cusFinaceRepo;
            _cusTransactionLogRepo = cusTransactionLogRepo;
            _capBus = capBus;
        }

        public CusManagerService(IEfRepository<Customer> cusRepo
            , IEfRepository<CusFinance> cusFinaceRepo
            , IEfRepository<CusTransactionLog> cusTransactionLogRepo)
        {
            _cusRepo = cusRepo;
            _cusFinaceRepo = cusFinaceRepo;
            _cusTransactionLogRepo = cusTransactionLogRepo;
        }

        public async Task Register(Customer customer, CusFinance cusFinance, CancellationToken cancellationToken = default)
        {
            await _cusRepo.InsertAsync(customer);
            await _cusFinaceRepo.InsertAsync(cusFinance);
        }

        public async Task Recharge(long customerId, decimal amount, CusTransactionLog cusTransactionLog, CancellationToken cancellationToken = default)
        {

            await _cusTransactionLogRepo.InsertAsync(cusTransactionLog);

            var regchargeInfo = new
            {
                ID = customerId
                ,
                Amount = amount
                ,
                TransactionLogId = cusTransactionLog.ID
            };

            await _capBus.PublishAsync(EbConsts.CustomerRechager, regchargeInfo);
        }
    }
}
