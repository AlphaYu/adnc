using Adnc.Infra.Application.Dtos;
using Adnc.Cus.Application.Contracts.Dtos;
using Adnc.Infra.IRepositories;
using AutoMapper;
using Adnc.Cus.Entities;

namespace Adnc.Cus.Application
{
    public class AdncCusProfile : Profile
    {
        public AdncCusProfile()
        {
            CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
            CreateMap<CustomerRegisterDto, Customer>();
            CreateMap<Customer, CustomerDto>();
        }
    }
}