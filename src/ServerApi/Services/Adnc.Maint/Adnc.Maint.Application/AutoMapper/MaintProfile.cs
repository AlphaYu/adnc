using Adnc.Shared.Rpc.Grpc.Messages;

namespace Adnc.Maint.Application.AutoMapper;

public class MaintProfile : Profile
{
    public MaintProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<OpsLogCreationDto, OperationLog>();
        CreateMap<OperationLog, OpsLogDto>();
        CreateMap<LoginLog, LoginLogDto>();
        CreateMap<LoggerLog, NlogLogDto>();
        CreateMap<SysNloglogProperty, NlogLogPropertyDto>();
        CreateMap<CfgCreationDto, SysCfg>();
        CreateMap<SysCfg, CfgDto>();

        CreateMap<DictCreationDto, SysDict>();
        CreateMap<SysDict, DictDto>();
        CreateMap<SysNotice, NoticeDto>().ReverseMap();

        CreateMap<DictDto, DictReply>();

    }
}