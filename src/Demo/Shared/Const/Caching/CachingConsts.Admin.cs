using Adnc.Shared;

namespace Adnc.Demo.Const.Caching.Admin;

public class CachingConsts
{
    //cache key
    public const string MenuListCacheKey = "admin:menus:list";
    public const string RoleMenuCodesCacheKey = GeneralConsts.RoleMenuCodesCacheKey;
    public const string DetpListCacheKey = "admin:depts:list";
    public const string DictOptionsListKey = "admin:dictoptions:list";
    public const string SysConfigListCacheKey = "admin:sysconfigs:list";

    public const string DictOptionsPreheatedKey = "admin:dictoptions:preheated";
    public const string SysConfigPreheatedKey = "admin:sysconfigs:preheated";

    //cache prefix
    public const string UserValidatedInfoKeyPrefix = GeneralConsts.UserValidatedInfoKeyPrefix;
    public const string UserFailCountKeyPrefix = "admin:users:validatedinfo:failcount";
    public const string DictOptionSingleKeyPrefix = "admin:dictoptions:single";

    //bloomfilter
    public const string BloomfilterOfAccountsKey = "admin:bloomfilter:accouts";
    public const string BloomfilterOfCacheKey = "admin:bloomfilter:cachekeys";
}
