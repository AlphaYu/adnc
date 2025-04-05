namespace Adnc.Demo.Admin.Application;

public sealed class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());
        CreateMap<MenuCreationDto, Menu>().ForMember(dest => dest.Params, opt => opt.MapFrom<KeyValuesToStringResolver>());
        CreateMap<MenuUpdationDto, Menu>().ForMember(dest => dest.Params, opt => opt.MapFrom<KeyValuesToStringResolver>());
        CreateMap<Menu, MenuDto>().ForMember(dest => dest.Params, opt => opt.MapFrom<StringToKeyValuesResolver>());
        CreateMap<MenuDto, MenuTreeDto>();
        CreateMap<RoleCreationDto, Role>();
        CreateMap<RoleUpdationDto, Role>();
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<RoleMenuRelation, RoleMenuRelationDto>();
        CreateMap<User, UserProfileDto>();
        CreateMap<UserCreationDto, User>();
        CreateMap<UserUpdationDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<OrganizationCreationDto, Organization>();
        CreateMap<OrganizationUpdationDto, Organization>();
        CreateMap<Organization, OrganizationDto>();
        CreateMap<OrganizationDto, OrganizationTreeDto>();
        CreateMap<OrganizationTreeDto, OptionTreeDto>()
            .ForMember(dest => dest.Label, opt => opt.MapFrom(x => x.Name))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(x => x.Id));
        CreateMap<DictCreationDto, Dict>();
        CreateMap<DictUpdationDto, Dict>();
        CreateMap<Dict, DictDto>();
        CreateMap<DictDataCreationDto, DictData>();
        CreateMap<DictDataUpdationDto, DictData>();
        CreateMap<DictData, DictDataDto>();
        CreateMap<SysConfigCreationDto, SysConfig>();
        CreateMap<SysConfigUpdationDto, SysConfig>();
        CreateMap<SysConfig, SysConfigDto>();

        CreateMap<SysConfigSimpleDto, SysConfigSimpleReply>();
        CreateMap<DictOptionDto, DictOptionReply>();

    }
}

public class KeyValuesToStringResolver : IValueResolver<MenuCreationDto, Menu, string>
{
    public string Resolve(MenuCreationDto source, Menu destination, string member, ResolutionContext context)
    {
        return source.Params.Select(x => $"{x.Key}={x.Value}").ToString("&");
    }
}

public class StringToKeyValuesResolver : IValueResolver<Menu, MenuDto, List<KeyValuePair<string, string>>>
{
    public List<KeyValuePair<string, string>> Resolve(Menu source, MenuDto destination, List<KeyValuePair<string, string>> member, ResolutionContext context)
    {
        if (source.Params.IsNullOrEmpty())
        {
            return [];
        }

        var keyValues = new List<KeyValuePair<string, string>>();
        foreach (var item in source.Params.Split("&"))
        {
            var array = item.Split("=");
            keyValues.Add(new KeyValuePair<string, string>(array[0], array[1]));
        }
        return keyValues;
    }
}
