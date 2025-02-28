using Adnc.Demo.Cust.Api.Application.Dtos;
using Adnc.Demo.Cust.Api.Repository.Entities;

namespace Adnc.Demo.Cust.Api;

public class CustProfile : Profile
{
    public CustProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<CustomerRegisterDto, Customer>();
        CreateMap<Customer, CustomerDto>();
    }
}