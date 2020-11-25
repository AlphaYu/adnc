using AutoMapper;
using Adnc.Cus.Application.Dtos;
using Adnc.Application.Shared.Dtos;
using Adnc.Cus.Core.Entities;
using Adnc.Core.Shared;

namespace Adnc.Cus.Application
{
    public class AdncCusProfile : Profile
    {
        public AdncCusProfile()
        {
            CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
            CreateMap<RegisterInputDto, Customer>();
        }
    }
}
