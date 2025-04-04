using Adnc.Demo.Remote.Event;
using Adnc.Demo.Remote.Http.Services;

namespace Adnc.Demo.Cust.Api.Application.Services.Implements;

/// <summary>
/// 客户管理服务
/// </summary>
public class CustomerAppService(IEfRepository<Customer> customerRepo, IEfRepository<TransactionLog> transactionLogRepo, IEventPublisher eventPublisher, IAdminRestClient adminRestClient)
    : AbstractAppService, ICustomerService
{
    public async Task<ServiceResult<IdDto>> CreateAsync(CustomerCreationDto input)
    {
        input.TrimStringFields();
        var exists = await customerRepo.AnyAsync(t => t.Account == input.Account);
        if (exists)
        {
            return Problem(HttpStatusCode.Forbidden, "客户账号已经存在");
        }

        var customer = Mapper.Map<Customer>(input, IdGenerater.GetNextId());
        //customer.Password = InfraHelper.Encrypt.Md5(customer.Password);

        customer.FinanceInfo = new Finance()
        {
            Account = customer.Account,
            Balance = 0,
            Id = customer.Id
        };

        await customerRepo.InsertAsync(customer);
        return new IdDto(customer.Id);
    }

    public async Task<ServiceResult<IdDto>> RechargeAsync(long id, decimal balance)
    {
        var customer = await customerRepo.FetchAsync(x => x.Id == id);
        if (customer is null)
        {
            return Problem(HttpStatusCode.NotFound, "客户账号不存在");
        }

        var transactionLog = new TransactionLog()
        {
            Id = IdGenerater.GetNextId(),
            CustomerId = customer.Id,
            Account = customer.Account,
            ExchangeType = ExchangeBehavior.Recharge,
            Remark = "",
            Amount = balance,
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
                                            .AndIf(input.CreateTime is not null && input.CreateTime.Length > 1, x => x.CreateTime >= input.CreateTime![0] && x.CreateTime <= input.CreateTime[1])
                                            .AndIf(input.Keywords.IsNotNullOrEmpty(), x => x.Account == input.Keywords);

        var count = await customerRepo.CountAsync(whereCondition);
        if (count == 0)
        {
            return new PageModelDto<CustomerDto>(input);
        }

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
                                                Balance = x.FinanceInfo.Balance
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
        return new PageModelDto<CustomerDto>(input, queryResult.Content.ToArray(), queryResult.TotalCount);
    }

    public async Task<ServiceResult<PageModelDto<TransactionLogDto>>> GetTransactionLogsPagedAsync(SearchPagedDto input)
    {
        input.TrimStringFields();
        var whereExpr = ExpressionCreator
                                            .New<TransactionLog>()
                                            .AndIf(input.CreateTime is not null && input.CreateTime.Length > 1, x => x.CreateTime >= input.CreateTime![0] && x.CreateTime <= input.CreateTime[1])
                                            .AndIf(input.Keywords.IsNotNullOrEmpty(), x => x.Account == input.Keywords);

        var count = await transactionLogRepo.CountAsync(whereExpr);
        if (count == 0)
        {
            return new PageModelDto<TransactionLogDto>(input);
        }

        var tranlogs = await transactionLogRepo
                                            .Where(whereExpr)
                                            .OrderByDescending(x => x.Id)
                                            .Skip(input.SkipRows())
                                            .Take(input.PageSize)
                                            .ToListAsync();

        var tranlogDtos = Mapper.Map<List<TransactionLogDto>>(tranlogs);

        var dictOptins = await adminRestClient.GetDictOptionsAsync("exchange_behavior,exchage_status");
        if (dictOptins is not null && dictOptins.Count > 0)
        {
            var behaviorOptions = dictOptins.FirstOrDefault(x => x.Code == "exchange_behavior")?.DictDataList ?? [];
            var statusOptions = dictOptins.FirstOrDefault(x => x.Code == "exchage_status")?.DictDataList ?? [];
            foreach (var tranlogDto in tranlogDtos)
            {
                var exchageStatus = tranlogDto.ExchageStatus.ToString();
                var exchangeType = tranlogDto.ExchangeType.ToString();
                tranlogDto.ExchangeTypeName = behaviorOptions.FirstOrDefault(x => x.Value == exchangeType)?.Label ?? string.Empty;
                tranlogDto.ExchageStatusName = statusOptions.FirstOrDefault(x => x.Value == exchageStatus)?.Label ?? string.Empty;
            }
        }
        return new PageModelDto<TransactionLogDto>(input, tranlogDtos, count);
    }
}
