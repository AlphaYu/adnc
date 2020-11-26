using AutoMapper;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Application.Services;
using Adnc.Application.Shared.Dtos;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared;

namespace Adnc.Usr.Application
{
    public class AdncUsrProfile : Profile
    {
        public AdncUsrProfile()
        {
            CreateMap(typeof(IPagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
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
            CreateMap<SysUser, UserDto>().ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<SysUser, UserProfileDto>().ForMember(d => d.DeptFullName, opt => opt.MapFrom(s => s.Dept.FullName));
            CreateMap<SysUser, UserValidateDto>();
            CreateMap<DeptSaveInputDto, SysDept>();
            CreateMap<SysDept, DeptDto>();
            CreateMap<SysDept, DeptNodeDto>();
            CreateMap<DeptDto, DeptNodeDto>();
        }
    }
}
