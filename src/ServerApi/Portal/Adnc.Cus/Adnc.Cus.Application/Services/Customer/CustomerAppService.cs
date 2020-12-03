using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Adnc.Cus.Application.Dtos;
using Adnc.Cus.Core.CoreServices;
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
        private readonly ICusManagerService _cusManagerService;
        private readonly IEfRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;
        public CustomerAppService(
             IEfRepository<Customer> customerRepo
            , ICusManagerService cusManagerService
            , IMapper mapper)
        {
            _customerRepo = customerRepo;
            _cusManagerService = cusManagerService;
            _mapper = mapper;
        }

        public async Task<AppSrvResult<SimpleDto<string>>> Register(RegisterInputDto inputDto)
        {
            var exists = await _customerRepo.ExistAsync(t => t.Account == inputDto.Account);
            if (exists)
                return Problem(HttpStatusCode.Forbidden, "该账号已经存在");

            var customer = _mapper.Map<Customer>(inputDto);
            customer.ID = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);

            var customerFinace = new CusFinance()
            {
                Account = customer.Account
               ,
                Balance = 0
                ,
                ID = customer.ID
            };

            await _cusManagerService.Register(customer, customerFinace);

            return new SimpleDto<string>(customer.ID.ToString());
        }

        public async Task<AppSrvResult<SimpleDto<string>>> Recharge(RechargeInputDto inputDto)
        {
            var customer = await _customerRepo.FindAsync(inputDto.ID);
            if (customer == null)
                return Problem(HttpStatusCode.NotFound, "不存在该账号");

            var cusTransactionLog = new CusTransactionLog()
            {
                ID = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
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

            await _cusManagerService.Recharge(customer.ID, inputDto.Amount, cusTransactionLog);

            return new SimpleDto<string>(cusTransactionLog.ID.ToString());
        }
    }
}
