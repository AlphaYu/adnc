using Adnc.Cus.Application.Services.Caching;
using Adnc.Shared.Application.Channels;
using Adnc.Shared.Repository.MongoEntities;

namespace Adnc.Cus.Application.Services;

/// <summary>
/// 客户管理服务
/// </summary>
public class CustomerAppService : AbstractAppService, ICustomerAppService
{
    private readonly IEfRepository<Customer> _customerRepo;
    private readonly IEfRepository<CustomerFinance> _cusFinaceRepo;
    private readonly IEfRepository<CustomerTransactionLog> _cusTransactionLogRepo;
    private readonly CacheService _cacheService;
    private readonly IEventPublisher _eventPublisher;

    public CustomerAppService(
        IEfRepository<Customer> customerRepo,
        IEfRepository<CustomerFinance> cusFinaceRepo,
        IEfRepository<CustomerTransactionLog> cusTransactionLogRepo,
        CacheService cacheService,
        IEventPublisher eventPublisher)
    {
        _customerRepo = customerRepo;
        _cusFinaceRepo = cusFinaceRepo;
        _cusTransactionLogRepo = cusTransactionLogRepo;
        _cacheService = cacheService;
        _eventPublisher = eventPublisher;
    }

    public async Task<AppSrvResult<CustomerDto>> RegisterAsync(CustomerRegisterDto input)
    {
        var exists = await _customerRepo.AnyAsync(t => t.Account == input.Account);
        if (exists)
            return Problem(HttpStatusCode.Forbidden, "该账号已经存在");

        var customer = Mapper.Map<Customer>(input, IdGenerater.GetNextId());
        customer.Password = InfraHelper.Security.MD5(customer.Password);

        customer.FinanceInfo = new CustomerFinance()
        {
            Account = customer.Account,
            Balance = 0,
            Id = customer.Id
        };

        await _customerRepo.InsertAsync(customer);

        var dto = Mapper.Map<CustomerDto>(customer);
        return dto;
    }

    public async Task<AppSrvResult<SimpleDto<string>>> RechargeAsync(long id, CustomerRechargeDto input)
    {
        var customer = await _customerRepo.FindAsync(id);
        if (customer == null)
            return Problem(HttpStatusCode.NotFound, "不存在该账号");

        var cusTransactionLog = new CustomerTransactionLog()
        {
            Id = IdGenerater.GetNextId(),
            CustomerId = customer.Id,
            Account = customer.Account,
            ExchangeType = ExchangeBehavior.Recharge,
            Remark = "",
            Amount = input.Amount,
            ExchageStatus = ExchageStatus.Processing
        };

        await _cusTransactionLogRepo.InsertAsync(cusTransactionLog);

        //发布充值事件
        var eventId = IdGenerater.GetNextId();
        var eventData = new CustomerRechargedEvent.EventData() { CustomerId = cusTransactionLog.CustomerId, TransactionLogId = cusTransactionLog.Id, Amount = cusTransactionLog.Amount };
        var eventSource = nameof(RechargeAsync);
        await _eventPublisher.PublishAsync(new CustomerRechargedEvent(eventId, eventData, eventSource));
        return new SimpleDto<string>(cusTransactionLog.Id.ToString());
    }

    public async Task<AppSrvResult> ProcessRechargingAsync(CustomerRechargedEvent eventDto, IMessageTracker tracker)
    {
        var customerId = eventDto.Data.CustomerId;
        var amount = eventDto.Data.Amount;
        var transactionLogId = eventDto.Data.TransactionLogId;

        var transLog = await _cusTransactionLogRepo.FindAsync(transactionLogId, noTracking: false);
        if (transLog is null)
            return AppSrvResult();

        var finance = await _cusFinaceRepo.FindAsync(customerId, noTracking: false);
        var originalBalance = finance.Balance;
        var newBalance = originalBalance + amount;

        finance.Balance = newBalance;
        await _cusFinaceRepo.UpdateAsync(finance);

        transLog.ExchageStatus = ExchageStatus.Finished;
        transLog.ChangingAmount = originalBalance;
        transLog.ChangedAmount = newBalance;
        await _cusTransactionLogRepo.UpdateAsync(transLog);

        await tracker?.MarkAsProcessedAsync(eventDto);
        return AppSrvResult();
    }

    public async Task<AppSrvResult> ProcessPayingAsync(long transactionLogId, long customerId, decimal amount)
    {
        var transLog = await _cusTransactionLogRepo.FindAsync(transactionLogId);
        if (transLog != null)
            return AppSrvResult();

        var account = await _customerRepo.FetchAsync(x => x.Account, x => x.Id == customerId);
        var finance = await _cusFinaceRepo.FindAsync(customerId, noTracking: false);
        var originalBalance = finance.Balance;
        var newBalance = originalBalance - amount;

        if (newBalance >= 0)
        {
            finance.Balance = newBalance;
            await _cusFinaceRepo.UpdateAsync(finance);

            transLog = new CustomerTransactionLog
            {
                Id = transactionLogId,
                CustomerId = customerId,
                Account = account,
                ChangingAmount = originalBalance,
                Amount = 0 - amount,
                ChangedAmount = newBalance,
                ExchangeType = ExchangeBehavior.Order,
                ExchageStatus = ExchageStatus.Finished
            };

            await _cusTransactionLogRepo.InsertAsync(transLog);
        }

        return AppSrvResult();
    }

    public async Task<AppSrvResult<PageModelDto<CustomerDto>>> GetPagedAsync(CustomerSearchPagedDto search)
    {
        var whereCondition = ExpressionCreator
                                            .New<Customer>()
                                            .AndIf(search.Id > 0, x => x.Id == search.Id)
                                            .AndIf(search.Account.IsNotNullOrEmpty(), x => x.Account == search.Account);

        var count = await _customerRepo.CountAsync(whereCondition);
        if (count == 0)
            return new PageModelDto<CustomerDto>(search);

        var customers = await _customerRepo
                                            .Where(whereCondition)
                                            .Select(x => new CustomerDto
                                            {
                                                Id = x.Id,
                                                Account = x.Account,
                                                Nickname = x.Nickname,
                                                Realname = x.Realname,
                                                CreateBy = x.CreateBy,
                                                CreateTime = x.CreateTime,
                                                FinanceInfoBalance = x.FinanceInfo == null ? 0 : x.FinanceInfo.Balance
                                            })
                                            .Skip(search.SkipRows())
                                            .Take(search.PageSize)
                                            .OrderByDescending(x => x.Id)
                                            .ToListAsync();

        return new PageModelDto<CustomerDto>(search, customers, count);
    }
}