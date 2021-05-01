using AutoMapper;
using Adnc.Application.Shared.Dtos;
using Adnc.Core.Shared;
using Adnc.Usr.Core.Entities;
using Adnc.Usr.Application.Contracts.Dtos;

namespace Adnc.Usr.Application
{
    public class AdncUsrProfile : Profile
    {
        public AdncUsrProfile()
        {
            CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
            CreateMap(typeof(ZTreeNodeDto<,>), typeof(Node<>)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<MenuCreationDto, SysMenu>();
            CreateMap<MenuUpdationDto, SysMenu>();
            CreateMap<SysMenu, MenuDto>().ReverseMap();
            CreateMap<MenuDto, MenuRouterDto>();
            CreateMap<SysMenu, MenuRouterDto>();
            CreateMap<SysMenu, MenuNodeDto>();
            CreateMap<MenuDto, MenuNodeDto>();
            CreateMap<SysRelation, RelationDto>();
            CreateMap<RoleCreationDto, SysRole>();
            CreateMap<RoleUpdationDto, SysRole>();
            CreateMap<SysRole, RoleDto>().ReverseMap();
            CreateMap<UserCreationDto, SysUser>();
            CreateMap<UserUpdationDto, SysUser>();
            CreateMap<SysUser, UserDto>().ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<DeptCreationDto, SysDept>();
            CreateMap<DeptUpdationDto, SysDept>();
            CreateMap<SysDept, DeptDto>();
            CreateMap<SysDept, DeptTreeDto>();
            CreateMap<DeptDto, DeptTreeDto>();
        }
    }
}
