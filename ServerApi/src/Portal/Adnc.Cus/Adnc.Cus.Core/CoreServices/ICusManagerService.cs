using System.Threading;
using System.Threading.Tasks;
using Adnc.Cus.Core.Entities;
using Adnc.Core.Shared;

namespace Adnc.Cus.Core.CoreServices
{
    public interface ICusManagerService : ICoreService
    {
        Task Register(Customer customer, CusFinance cusFinance, CancellationToken cancellationToken = default);
        Task Recharge(long customerId, decimal amount, CusTransactionLog cusTransactionLog, CancellationToken cancellationToken = default);
    }
}
