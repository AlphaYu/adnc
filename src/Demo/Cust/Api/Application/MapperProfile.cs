using Adnc.Demo.Cust.Api.Application.Contracts.Dtos.Customer;
using AutoMapper;

namespace Adnc.Demo.Cust.Api.Application;

public class CustProfile : Profile
{
    public CustProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<CustomerCreationDto, Customer>();
        CreateMap<Customer, CustomerDto>();
        CreateMap<TransactionLog, TransactionLogDto>();
    }
}
