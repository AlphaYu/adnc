using Adnc.Application.Shared;

namespace Adnc.Usr.Application.Contracts.Consts
{
    public class EasyCachingConsts : BaseEasyCachingConsts
    {
        public const string MenuListCacheKey = "adnc:usr:menus:list";
        public const string MenuRelationCacheKey = "adnc:usr:menus:relation";
        public const string MenuCodesCacheKey = "adnc:usr:menus:codes";
        public const string DetpListCacheKey = "adnc:usr:depts";
        public const string SearchUsersKeyPrefix = "adnc:usr:users";
        public const string SearchOperationLogsKeyPrefix = "adnc:usr:logs";
        public const string RoleAllCacheKey = "adnc:usr:roles";
        public const string UserLoginInfoKey = "adnc:usr:users:logininfo:{0}";
        public const string UserLoginInfoKeyPrefix = "adnc:usr:users:logininfo";
    }
}
