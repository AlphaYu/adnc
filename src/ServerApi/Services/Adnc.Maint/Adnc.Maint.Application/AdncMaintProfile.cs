﻿namespace Adnc.Maint.Application;

public class AdncMaintProfile : Profile
{
    public AdncMaintProfile()
    {
        CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<OpsLogCreationDto, OperationLog>();
        CreateMap<OperationLog, OpsLogDto>();
        CreateMap<LoginLog, LoginLogDto>();
        CreateMap<LoggingLog, NlogLogDto>();
        CreateMap<SysNloglogProperty, NlogLogPropertyDto>();
        CreateMap<CfgCreationDto, SysCfg>();
        CreateMap<SysCfg, CfgDto>();

        CreateMap<DictCreationDto, SysDict>();
        CreateMap<SysDict, DictDto>();
        CreateMap<SysNotice, NoticeDto>().ReverseMap();

        //CreateMap<TaskSaveInputDto, SysTask>();
        //CreateMap<SysTask, TaskDto>();
        //CreateMap<SysTaskLog, TaskLogDto>().ReverseMap();
    }
}