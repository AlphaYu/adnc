using Adnc.Demo.Maint.Application.Contracts.Dtos.Log;
using AutoMapper;

namespace Adnc.Demo.Maint.Application;

public class MaintProfile : Profile
{
    public MaintProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<OperationLog, OperationLogDto>();
        CreateMap<LoginLog, LoginLogDto>();
    }
}
