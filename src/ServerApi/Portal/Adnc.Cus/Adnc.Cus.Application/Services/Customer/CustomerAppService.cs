using System;
using System.Net;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Adnc.Cus.Application.Dtos;
using Adnc.Cus.Core.Services;
using Adnc.Cus.Core.Entities;
using Adnc.Infr.Common.Helper;
using Adnc.Core.Shared.IRepositories;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Infr.Common.Extensions;

namespace Adnc.Cus.Application.Services
{
    /// <summary>
    /// 客户管理服务
    /// </summary>
    public class CustomerAppService : AppService, ICustomerAppService
    {
        private readonly CustomerManagerService _cusManagerService;
        private readonly IEfRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customerRepo"></param>
        /// <param name="cusManagerService"></param>
        /// <param name="mapper"></param>
        public CustomerAppService(
             IEfRepository<Customer> customerRepo
            , CustomerManagerService cusManagerService
            , IMapper mapper)
        {
            _customerRepo = customerRepo;
            _cusManagerService = cusManagerService;
            _mapper = mapper;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AppSrvResult<CustomerDto>> RegisterAsync(CustomerRegisterDto input)
        {
            var exists = await _customerRepo.AnyAsync(t => t.Account == input.Account);
            if (exists)
                return Problem(HttpStatusCode.Forbidden, "该账号已经存在");

            var customer = _mapper.Map<Customer>(input);

            customer.Id = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);
            customer.FinanceInfo = new CustomerFinance()
            {
                Account = customer.Account
               ,
                Balance = 0
                ,
                Id = customer.Id
            };

            await _customerRepo.InsertAsync(customer);

            var dto = _mapper.Map<CustomerDto>(customer);
            return dto;
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AppSrvResult<SimpleDto<string>>> RechargeAsync(long id, CustomerRechargeDto input)
        {
            var customer = await _customerRepo.FindAsync(id);
            if (customer == null)
                return Problem(HttpStatusCode.NotFound, "不存在该账号");

            var cusTransactionLog = new CustomerTransactionLog()
            {
                Id = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                ,
                CustomerId = customer.Id
                ,
                Account = customer.Account
                ,
                ExchangeType = ExchangeTypeEnum.Recharge
                ,
                Remark = ""
                ,
                Amount = input.Amount
                ,
                ExchageStatus = ExchageStatusEnum.Processing
            };

            await _cusManagerService.RechargeAsync(cusTransactionLog);

            return new SimpleDto<string>(cusTransactionLog.Id.ToString());
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<AppSrvResult<PageModelDto<CustomerDto>>> GetPagedAsync(CustomerSearchPagedDto search)
        {
            Expression<Func<Customer, bool>> whereCondition = x => true;
            if (search.Id.ToLong() > 0)
                whereCondition = whereCondition.And(x => x.Id == search.Id.ToLong());
            if (search.Account.IsNotNullOrEmpty())
                whereCondition = whereCondition.And(x => x.Account == search.Account);

            var count = await _customerRepo.CountAsync(whereCondition);
            if (count == 0)
                return new PageModelDto<CustomerDto>(search);

            //这里用直接用dapper更方便_customerRepo.QueryAsync(sql)
            var customers = await _customerRepo
                                .Where(whereCondition)
                                .Select(x => new CustomerDto
                                {
                                    Id = x.Id.ToString()
                                ,
                                    Account = x.Account
                                ,
                                    Nickname = x.Nickname
                                ,
                                    Realname = x.Realname
                                ,
                                    CreateBy = x.CreateBy
                                ,
                                    CreateTime = x.CreateTime
                                ,
                                    FinanceInfoBalance = x.FinanceInfo.Balance
                                })
                                .Skip(search.SkipRows())
                                .Take(search.PageSize)
                                .OrderByDescending(x => x.Id)
                                .ToListAsync();

            return new PageModelDto<CustomerDto>(search, customers, count);
        }
    }
}
