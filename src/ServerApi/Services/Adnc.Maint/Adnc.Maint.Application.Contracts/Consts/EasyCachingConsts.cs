using Adnc.Application.Shared;

namespace Adnc.Maint.Application.Contracts.Consts
{
    public class EasyCachingConsts: BaseEasyCachingConsts
    {
        public const string DictListCacheKey = "adnc:maint:dicts";
        public const string CfgListCacheKey = "adnc:maint:cfgs";
    }
}
