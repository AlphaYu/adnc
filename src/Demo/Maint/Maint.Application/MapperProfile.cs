using Adnc.Demo.Shared.Remote.Grpc.Messages;

namespace Adnc.Demo.Maint.Application;

public class MaintProfile : Profile
{
    public MaintProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<OpsLogCreationDto, OperationLog>();
        CreateMap<OperationLog, OpsLogDto>();
        CreateMap<LoginLog, LoginLogDto>();
        //CreateMap<LoggerLog, NlogLogDto>();
        //CreateMap<NloglogProperty, NlogLogPropertyDto>();
    }
}