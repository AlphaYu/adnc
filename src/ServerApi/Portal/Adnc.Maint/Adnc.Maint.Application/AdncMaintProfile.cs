using AutoMapper;
using Adnc.Maint.Application.Dtos;
using Adnc.Common.Models;
using Adnc.Core.Maint.Entities;
using Adnc.Application.Shared.Dtos;
using Adnc.Maint.Core.Entities;

namespace Adnc.Maint.Application
{
    public class AdncMaintProfile : Profile
    {
        public AdncMaintProfile()
        {
            CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>));
            CreateMap<OpsLogSaveInputDto, SysOperationLog>();
            CreateMap<SysOperationLog, OpsLogDto>();
            CreateMap<SysLoginLog, LoginLogDto>();
            CreateMap<SysNloglog, NlogLogDto>();
            CreateMap<CfgSaveInputDto, SysCfg>();
            CreateMap<SysCfg, CfgDto>();


            CreateMap<DictSaveInputDto, SysDict>();
            CreateMap<SysDict, DictDto>();
            CreateMap<SysNotice, NoticeDto>().ReverseMap();

            CreateMap<OpsLogSaveInputDto, SysOperationLog>();

            CreateMap<SysOperationLog, OpsLogDto>();
            CreateMap<SysLoginLog, LoginLogDto>();
            CreateMap<SysNloglog, NlogLogDto>();

            CreateMap<TaskSaveInputDto, SysTask>();
            CreateMap<SysTask, TaskDto>();
            CreateMap<SysTaskLog, TaskLogDto>().ReverseMap();
        }
    }
}
