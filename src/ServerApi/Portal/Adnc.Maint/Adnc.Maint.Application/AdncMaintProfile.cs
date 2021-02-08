using AutoMapper;
using Adnc.Maint.Application.Dtos;
using Adnc.Core.Maint.Entities;
using Adnc.Application.Shared.Dtos;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared;

namespace Adnc.Maint.Application
{
    public class AdncMaintProfile : Profile
    {
        public AdncMaintProfile()
        {
            CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
            CreateMap<OpsLogCreationDto, SysOperationLog>();
            CreateMap<SysOperationLog, OpsLogDto>();
            CreateMap<SysLoginLog, LoginLogDto>();
            CreateMap<SysNloglog, NlogLogDto>();
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
}
