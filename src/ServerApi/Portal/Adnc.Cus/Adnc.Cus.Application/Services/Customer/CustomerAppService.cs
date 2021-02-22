using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Adnc.Cus.Application.Dtos;
using Adnc.Cus.Core.Services;
using Adnc.Cus.Core.Entities;
using Adnc.Infr.Common.Helper;
using Adnc.Core.Shared.IRepositories;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared;
using Adnc.Application.Shared.Services;

namespace Adnc.Cus.Application.Services
{
    public class CustomerAppService :AppService,ICustomerAppService
    {
        private readonly CustomerManagerService _cusManagerService;
        private readonly IEfRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;
        public CustomerAppService(
             IEfRepository<Customer> customerRepo
            , CustomerManagerService cusManagerService
            , IMapper mapper)
        {
            _customerRepo = customerRepo;
            _cusManagerService = cusManagerService;
            _mapper = mapper;
        }

        public async Task<AppSrvResult<SimpleDto<string>>> Register(CustomerRegisterDto inputDto)
        {
            var exists = await _customerRepo.AnyAsync(t => t.Account == inputDto.Account);
            if (exists)
                return Problem(HttpStatusCode.Forbidden, "该账号已经存在");

            var customer = _mapper.Map<Customer>(inputDto);
            customer.Id = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);

            var customerFinace = new CusFinance()
            {
                Account = customer.Account
               ,
                Balance = 0
                ,
                Id = customer.Id
            };

            await _cusManagerService.RegisterAsync(customer, customerFinace);

            return new SimpleDto<string>(customer.Id.ToString());
        }

        public async Task<AppSrvResult<SimpleDto<string>>> Recharge(CustomerRechargeDto inputDto)
        {
            var customer = await _customerRepo.FindAsync(inputDto.ID);
            if (customer == null)
                return Problem(HttpStatusCode.NotFound, "不存在该账号");

            var cusTransactionLog = new CusTransactionLog()
            {
                Id = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                ,
                CustomerId = customer.Id
                ,
                Account = customer.Account
                ,
                ExchangeType = "100"
                ,
                Remark = ""
                ,
                Amount = inputDto.Amount
                ,
                ExchageStatus = "10"
            };

            await _cusManagerService.RechargeAsync(cusTransactionLog);

            return new SimpleDto<string>(cusTransactionLog.Id.ToString());
        }
    }
}
