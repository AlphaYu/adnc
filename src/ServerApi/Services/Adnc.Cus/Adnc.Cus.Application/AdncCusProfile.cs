namespace Adnc.Cus.Application;

public class AdncCusProfile : Profile
{
    public AdncCusProfile()
    {
        CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<CustomerRegisterDto, Customer>();
        CreateMap<Customer, CustomerDto>();
    }
}