namespace Adnc.Demo.Shared.Const.Caching.Usr;

public class CachingConsts
{
    //cache key
    public const string MenuListCacheKey = "adnc:usr:menus:list";

    public const string RoleMenuCodesCacheKey = "adnc:usr:role:menus:codes";

    public const string DetpListCacheKey = "adnc:usr:depts:list";

    //cache prefix
    public const string UserValidatedInfoKeyPrefix = "adnc:usr:users:validatedinfo";

    //bloomfilter
    public const string BloomfilterOfAccountsKey = "adnc:usr:bloomfilter:accouts";
    public const string BloomfilterOfCacheKey = "adnc:usr:bloomfilter:cachekeys";
}