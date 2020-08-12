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

        public const string TopicName = "sysname_easycaching_synchronize_topic";
        public const string LocalCaching = "l1";
        public const string RemoteCaching = "r1";
        public const string HybridCaching = "h1";

        public const string DetpListCacheKey = "sysname:service:depts";

        public const string DictListCacheKey = "sysname:service:dicts";
        public const string CfgListCacheKey = "sysname:service:cfgs";

        public const string MenuKesPrefix = "sysname:service:menus";
        public const string MenuListCacheKey = "sysname:service:menus:list";
        public const string MenuRouterCacheKey = "sysname:service:menus:router";
        public const string MenuRelationCacheKey = "sysname:service:menus:relation";
        public const string MenuCodesCacheKey = "sysname:service:menus:codes";

        public const string SearchUsersKeyPrefix = "sysname:servcie:users";
        public const string SearchOperationLogsKeyPrefix = "sysname:servcie:logs";

    }
}
