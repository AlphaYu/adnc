namespace Adnc.Demo.Cust.Api.Application.Services.Implements;

/// <summary>
/// 客户管理服务
/// </summary>
public class CustomerAppService(IEfRepository<Customer> customerRepo, IEfRepository<TransactionLog> transactionLogRepo, IEventPublisher eventPublisher)
    : AbstractAppService, ICustomerService
{
    public async Task<ServiceResult<IdDto>> CreateAsync(CustomerCreationDto input)
    {
        input.TrimStringFields();
        var exists = await customerRepo.AnyAsync(t => t.Account == input.Account);
        if (exists)
            return Problem(HttpStatusCode.Forbidden, "客户账号已经存在");

        var customer = Mapper.Map<Customer>(input, IdGenerater.GetNextId());
        customer.Password = InfraHelper.Encrypt.Md5(customer.Password);

        customer.FinanceInfo = new Finance()
        {
            Account = customer.Account,
            Balance = 0,
            Id = customer.Id
        };

        await customerRepo.InsertAsync(customer);
        return new IdDto(customer.Id);
    }

    public async Task<ServiceResult<IdDto>> RechargeAsync(long id, CustomerRechargeDto input)
    {
        input.TrimStringFields();
        var customer = await customerRepo.FetchAsync(x => x.Id == id);
        if (customer is null)
            return Problem(HttpStatusCode.NotFound, "客户账号不存在");

        var transactionLog = new TransactionLog()
        {
            Id = IdGenerater.GetNextId(),
            CustomerId = customer.Id,
            Account = customer.Account,
            ExchangeType = ExchangeBehavior.Recharge,
            Remark = "",
            Amount = input.Amount,
            ExchageStatus = ExchageStatus.Processing
        };
        await transactionLogRepo.InsertAsync(transactionLog);

        //发布充值事件
        var customerRechargedEvent = new CustomerRechargedEvent
        {
            Id = IdGenerater.GetNextId(),
            EventSource = nameof(RechargeAsync),
            CustomerId = transactionLog.CustomerId,
            TransactionLogId = transactionLog.Id,
            Amount = transactionLog.Amount
        };
        await eventPublisher.PublishAsync(customerRechargedEvent);

        return new IdDto(transactionLog.Id);
    }

    public async Task<ServiceResult<PageModelDto<CustomerDto>>> GetPagedAsync(SearchPagedDto input)
    {
        input.TrimStringFields();
        var whereCondition = ExpressionCreator
                                            .New<Customer>()
                                            .AndIf(input.Keywords.IsNotNullOrEmpty(), x => x.Account == input.Keywords);

        var count = await customerRepo.CountAsync(whereCondition);
        if (count == 0)
            return new PageModelDto<CustomerDto>(input);

        var customerDtos = await customerRepo
                                            .Where(whereCondition)
                                            .Select(x => new CustomerDto
                                            {
                                                Id = x.Id,
                                                Account = x.Account,
                                                Nickname = x.Nickname,
                                                Realname = x.Realname,
                                                CreateBy = x.CreateBy,
                                                CreateTime = x.CreateTime,
                                                FinanceInfoBalance = x.FinanceInfo.Balance
                                            })
                                            .OrderByDescending(x => x.Id)
                                            .Skip(input.SkipRows())
                                            .Take(input.PageSize)
                                            .ToListAsync();

        return new PageModelDto<CustomerDto>(input, customerDtos, count);
    }

    public async Task<ServiceResult<PageModelDto<CustomerDto>>> GetPagedBySqlAsync(SearchPagedDto input)
    {
        input.TrimStringFields();
        var where = new StringBuilder(100)
                                    .AppendIf(!string.IsNullOrEmpty(input.Keywords), " AND account = @Keywords")
                                    .ToSqlWhereString();
        var orderBy = " ORDER BY id Desc";

        var queryCondition = new QueryCondition(where, orderBy, null, input);
        var queryResult = await customerRepo.GetPagedCustmersBySqlAsync<CustomerDto>(queryCondition, input.SkipRows(), input.PageSize);
        return new PageModelDto<CustomerDto>(input, Enumerable.ToArray<CustomerDto>(queryResult.Content), (int)queryResult.TotalCount);
    }

    public async Task<ServiceResult<PageModelDto<TransactionLogDto>>> GetTransactionLogsPagedAsync(SearchPagedDto input)
    {
        input.TrimStringFields();
        var whereExpr = ExpressionCreator
                                            .New<TransactionLog>()
                                            .AndIf(input.Keywords.IsNotNullOrEmpty(), x => x.Account == input.Keywords);

        var count = await transactionLogRepo.CountAsync(whereExpr);
        if (count == 0)
            return new PageModelDto<TransactionLogDto>(input);

        var tranlogs = await transactionLogRepo
                                            .Where(whereExpr)
                                            .OrderByDescending(x => x.Id)
                                            .Skip(input.SkipRows())
                                            .Take(input.PageSize)
                                            .ToListAsync();

        var tranlogDtos = Mapper.Map<List<TransactionLogDto>>(tranlogs);

        return new PageModelDto<TransactionLogDto>(input, tranlogDtos, count);
    }
}