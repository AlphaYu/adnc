using AutoMapper;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Application.Services;
using Adnc.Common.Models;
using Adnc.Application.Shared.Dtos;
using Adnc.Usr.Core.Entities;

namespace Adnc.Usr.Application
{
    public class AdncUsrProfile : Profile
    {
        public AdncUsrProfile()
        {
            CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>));
            CreateMap(typeof(ZTreeNodeDto<,>), typeof(Node<>)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<MenuSaveInputDto, SysMenu>();
            CreateMap<SysMenu, MenuDto>().ReverseMap();
            CreateMap<MenuDto, RouterMenuDto>();
            CreateMap<SysMenu, RouterMenuDto>();
            CreateMap<SysMenu, MenuNodeDto>();
            CreateMap<MenuDto, MenuNodeDto>();
            CreateMap<SysRelation, RelationDto>();
            CreateMap<RoleSaveInputDto, SysRole>();
            CreateMap<SysRole, RoleDto>().ReverseMap();
            CreateMap<UserSaveInputDto, SysUser>();
            CreateMap<SysUser, UserDto>().ReverseMap();
            CreateMap<SysUser, UserProfileDto>();
            CreateMap<SysUser, UserValidateDto>();
            CreateMap<DeptSaveInputDto, SysDept>();
            CreateMap<SysDept, DeptDto>();
            CreateMap<SysDept, DeptNodeDto>();
            CreateMap<DeptDto, DeptNodeDto>();
        }
    }
}
