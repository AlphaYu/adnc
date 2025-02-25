namespace Adnc.Demo.Shared.Const.Caching.Usr;

public class CachingConsts
{
    //cache key
    public const string MenuListCacheKey = "adnc:usr:menus:list";

    public const string MenuTreeListCacheKey = "adnc:usr:menus:tree";
    public const string MenuRelationCacheKey = "adnc:usr:menus:relation";
    public const string MenuCodesCacheKey = "adnc:usr:menus:codes";

    public const string DetpListCacheKey = "adnc:usr:depts:list";
    public const string DetpTreeListCacheKey = "adnc:usr:depts:tree";
    public const string DetpSimpleTreeListCacheKey = "adnc:usr:depts:tree:simple";

    public const string RoleListCacheKey = "adnc:usr:roles:list";

    //cache prefix
    public const string UserValidatedInfoKeyPrefix = "adnc:usr:users:validatedinfo";

    //bloomfilter
    public const string BloomfilterOfAccountsKey = "adnc:usr:bloomfilter:accouts";
    public const string BloomfilterOfCacheKey = "adnc:usr:bloomfilter:cachekeys";
}