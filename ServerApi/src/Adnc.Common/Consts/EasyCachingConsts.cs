using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Common.Consts
{
    public class EasyCachingConsts
    {
        public const int OneYear = 365 * 24 * 60 * 60;
        public const int OneMonth = 30 * 24 * 60 * 60;
        public const int OneDay = 24 * 60 * 60;

        public const string TopicName = "adnc_usr_synchronize_topic";
        public const string LocalCaching = "l1";
        public const string RemoteCaching = "r1";
        public const string HybridCaching = "h1";

        //usr model
        public const string MenuListCacheKey = "adnc:usr:menus:list";
        public const string MenuRelationCacheKey = "adnc:usr:menus:relation";
        public const string MenuCodesCacheKey = "adnc:usr:menus:codes";
        public const string DetpListCacheKey = "adnc:usr:depts";
        public const string SearchUsersKeyPrefix = "adnc:usr:users";
        public const string SearchOperationLogsKeyPrefix = "adnc:usr:logs";

        public const string DictListCacheKey = "adnc:usr:dicts";
        public const string CfgListCacheKey = "adnc:usr:cfgs";



        
        

    }
}
