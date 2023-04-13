using Adnc.Demo.Shared.Rpc.Grpc.Messages;

namespace Adnc.Demo.Maint.Application;

public class MaintProfile : Profile
{
    public MaintProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<OpsLogCreationDto, OperationLog>();
        CreateMap<OperationLog, OpsLogDto>();
        CreateMap<LoginLog, LoginLogDto>();
        CreateMap<LoggerLog, NlogLogDto>();
        CreateMap<NloglogProperty, NlogLogPropertyDto>();
        CreateMap<CfgCreationDto, Cfg>();
        CreateMap<Cfg, CfgDto>();

        CreateMap<DictCreationDto, Dict>();
        CreateMap<Dict, DictDto>();
        CreateMap<Notice, NoticeDto>().ReverseMap();

        CreateMap<DictDto, DictReply>();

    }
}