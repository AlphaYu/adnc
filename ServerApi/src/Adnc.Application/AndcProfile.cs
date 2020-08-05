using AutoMapper;
using Adnc.Application.Dtos;
using Adnc.Application.Services;
using Adnc.Common.Models;
using Adnc.Core.Entities;

namespace Adnc.Application
{
    public class AdncProfile : Profile
    {
        public AdncProfile()
        {
            CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>));
            CreateMap(typeof(ZTreeNodeDto<,>), typeof(Node<>)).IgnoreAllPropertiesWithAnInaccessibleSetter();
         
            CreateMap<CfgSaveInputDto, SysCfg>();
            CreateMap<SysCfg, CfgDto>();

            CreateMap<DeptSaveInputDto, SysDept>();
            CreateMap<SysDept, DeptDto>();
            CreateMap<SysDept, DeptNodeDto>();

            CreateMap<DictSaveInputDto, SysDict>();
            CreateMap<SysDict, DictDto>();

            //CreateMap<SysFileInfo, FileInfoDto>();

            CreateMap<MenuSaveInputDto, SysMenu>();
            CreateMap<SysMenu, MenuDto>().ReverseMap();
            CreateMap<SysMenu, RouterMenuDto>();
            CreateMap<SysMenu, MenuNodeDto>();

            CreateMap<SysNotice, NoticeDto>().ReverseMap();

            CreateMap<OpsLogSaveInputDto, SysOperationLog>();
            CreateMap<SysOperationLog, OpsLogDto>();
            CreateMap<SysLoginLog, LoginLogDto>();
            CreateMap<SysNloglog, NlogLogDto>();

            CreateMap<SysRelation, RelationDto>();

            CreateMap<RoleSaveInputDto, SysRole>();
            CreateMap<SysRole, RoleDto>().ReverseMap();

            CreateMap<TaskSaveInputDto, SysTask>();
            CreateMap<SysTask, TaskDto>();
            CreateMap<SysTaskLog, TaskLogDto>().ReverseMap();

            CreateMap<UserSaveInputDto, SysUser>();
            CreateMap<SysUser, UserDto>().ReverseMap();
            CreateMap<SysUser, UserProfileDto>();
            CreateMap<SysUser, UserValidateDto>();
        }
    }
}
