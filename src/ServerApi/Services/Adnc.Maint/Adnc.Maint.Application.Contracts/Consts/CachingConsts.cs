using Adnc.Application.Shared.Consts;

namespace Adnc.Maint.Application.Contracts.Consts
{
    public class CachingConsts: SharedCachingConsts
    {
        public const string DictListCacheKey = "adnc:maint:dicts:list";

        public const string CfgListCacheKey = "adnc:maint:cfgs:list";
        public const string CfgSingleCacheKeyPrefix = "adnc:maint:cfgs";
    }
}
