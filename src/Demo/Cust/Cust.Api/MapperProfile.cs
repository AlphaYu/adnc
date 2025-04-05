namespace Adnc.Demo.Cust.Api;

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
