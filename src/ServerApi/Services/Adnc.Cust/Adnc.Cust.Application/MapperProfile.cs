namespace Adnc.Cust.Application.AutoMapper;

public class CustProfile : Profile
{
    public CustProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<CustomerRegisterDto, Customer>();
        CreateMap<Customer, CustomerDto>();
    }
}