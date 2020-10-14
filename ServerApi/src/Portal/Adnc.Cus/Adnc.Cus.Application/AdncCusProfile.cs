using AutoMapper;
using Adnc.Cus.Application.Dtos;
using Adnc.Common.Models;
using Adnc.Application.Shared.Dtos;
using Adnc.Cus.Core.Entities;

namespace Adnc.Cus.Application
{
    public class AdncCusProfile : Profile
    {
        public AdncCusProfile()
        {
            CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>));
            CreateMap<RegisterInputDto, Customer>();
        }
    }
}
