namespace Adnc.Application.Shared.Consts
{
    public class SharedCachingConsts
    {
        public const string LinkChar = ":";

        public const int OneYear = 365 * 24 * 60 * 60;
        public const int OneMonth = 30 * 24 * 60 * 60;
        public const int OneWeek = 7 * 24 * 60 * 60;
        public const int OneDay = 24 * 60 * 60;

        //public const string TopicName = "adnc_synchronize_topic";
        //public const string LocalCaching = "l1";
        //public const string RemoteCaching = "r1";
        //public const string HybridCaching = "h1";
        public const string WorkerIdSortedSetCacheKey = "adnc:{0}:workid:sortedset";
    }
}