using Adnc.Demo.Cust.Api.Application.Cache;
using Adnc.Demo.Cust.Api.Application.Dtos;
using Adnc.Demo.Cust.Api.Repository;
using Adnc.Demo.Cust.Api.Repository.Entities;

namespace Adnc.Demo.Cust.Api.Application.Services.Implements;

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
        input.TrimStringFields();
        var exists = await _customerRepo.AnyAsync(t => t.Account == input.Account);
        if (exists)
            return Problem(HttpStatusCode.Forbidden, "该账号已经存在");

        var customer = Mapper.Map<Customer>(input, IdGenerater.GetNextId());
        customer.Password = InfraHelper.Encrypt.Md5(customer.Password);

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
        input.TrimStringFields();
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
        var customerRechargedEvent = new CustomerRechargedEvent
        {
            Id = IdGenerater.GetNextId(),
            EventSource = MethodBase.GetCurrentMethod()?.GetMethodName() ?? string.Empty,
            CustomerId = cusTransactionLog.CustomerId,
            TransactionLogId = cusTransactionLog.Id,
            Amount = cusTransactionLog.Amount
        };
        await _eventPublisher.PublishAsync(customerRechargedEvent);

        return new SimpleDto<string>(cusTransactionLog.Id.ToString());
    }

    public async Task<AppSrvResult<PageModelDto<CustomerDto>>> GetPagedAsync(CustomerSearchPagedDto search)
    {
        search.TrimStringFields();

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

    public async Task<AppSrvResult<PageModelDto<CustomerDto>>> GetPagedBySqlAsync(CustomerSearchPagedDto search)
    {
        search.TrimStringFields();
        var where = new StringBuilder(100)
            .AppendIf(search.Id > 0, " AND id=@id")
            .AppendIf(!string.IsNullOrEmpty(search.Account), " AND account like @account")
            .ToSqlWhereString();
        var orderBy = " ORDER BY id Desc";

        search.Account += "%";
        var queryCondition = new QueryCondition(where, orderBy, null, search);
        var queryResult = await _customerRepo.GetPagedCustmersBySqlAsync<CustomerDto>(queryCondition, search.SkipRows(), search.PageSize);
        return new PageModelDto<CustomerDto>(search, queryResult.Content.ToArray(), queryResult.TotalCount);
    }
}