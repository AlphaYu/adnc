namespace Adnc.Shared.Consts.Caching.Com
{
    public class CachingConsts
    {
        public const string LinkChar = ":";

        public const int OneYear = 365 * 24 * 60 * 60;
        public const int OneMonth = 30 * 24 * 60 * 60;
        public const int OneWeek = 7 * 24 * 60 * 60;
        public const int OneDay = 24 * 60 * 60;

        public const string WorkerIdSortedSetCacheKey = "adnc:{0}:workids";
    }
}