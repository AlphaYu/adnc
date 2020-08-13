using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application
{
    public class EasyCachingConsts
    {
        public const int OneYear = 365 * 24 * 60 * 60;
        public const int OneMonth = 30 * 24 * 60 * 60;
        public const int OneDay = 24 * 60 * 60;

        public const string TopicName = "adnc_sys_synchronize_topic";
        public const string LocalCaching = "l1";
        public const string RemoteCaching = "r1";
        public const string HybridCaching = "h1";

        public const string DetpListCacheKey = "adnc:sys:depts";

        public const string DictListCacheKey = "adnc:sys:dicts";
        public const string CfgListCacheKey = "adnc:sys:cfgs";

        public const string MenuListCacheKey = "adnc:sys:menus:list";
        public const string MenuRelationCacheKey = "adnc:sys:menus:relation";
        public const string MenuCodesCacheKey = "adnc:sys:menus:codes";

        public const string SearchUsersKeyPrefix = "adnc:sys:users";
        public const string SearchOperationLogsKeyPrefix = "adnc:sys:logs";

    }
}
