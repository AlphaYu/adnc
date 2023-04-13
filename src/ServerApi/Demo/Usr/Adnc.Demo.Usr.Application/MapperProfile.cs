using Adnc.Demo.Shared.Rpc.Grpc.Messages;

namespace Adnc.Demo.Usr.Application;

public sealed class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap(typeof(ZTreeNodeDto<,>), typeof(Node<>)).IgnoreAllPropertiesWithAnInaccessibleSetter();
        CreateMap<MenuCreationDto, Menu>();
        CreateMap<MenuUpdationDto, Menu>();
        CreateMap<Menu, MenuDto>().ReverseMap();
        CreateMap<MenuDto, MenuRouterDto>();
        CreateMap<Menu, MenuRouterDto>();
        CreateMap<Menu, MenuNodeDto>();
        CreateMap<MenuDto, MenuNodeDto>();
        CreateMap<RoleRelation, RelationDto>();
        CreateMap<RoleCreationDto, Role>();
        CreateMap<RoleUpdationDto, Role>();
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<UserCreationDto, User>();
        CreateMap<UserUpdationDto, User>();
        CreateMap<User, UserDto>().ForMember(dest => dest.Password, opt => opt.Ignore());
        CreateMap<OrganizationCreationDto, Organization>();
        CreateMap<OrganizationUpdationDto, Organization>();
        CreateMap<Organization, OrganizationDto>();
        CreateMap<Organization, OrganizationTreeDto>();

        CreateMap<LoginRequest, UserLoginDto>();
        CreateMap<OrganizationTreeDto, DeptReply>();
    }
}