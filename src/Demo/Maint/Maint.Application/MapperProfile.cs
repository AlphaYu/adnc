using Adnc.Demo.Remote.Grpc.Messages;

namespace Adnc.Demo.Maint.Application;

public class MaintProfile : Profile
{
    public MaintProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<OperationLog, OperationLogDto>();
        CreateMap<LoginLog, LoginLogDto>();
        //CreateMap<LoggerLog, NlogLogDto>();
        //CreateMap<NloglogProperty, NlogLogPropertyDto>();
    }
}