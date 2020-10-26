using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Shared
{
    public class BaseEasyCachingConsts
    {
        public const int OneYear = 365 * 24 * 60 * 60;
        public const int OneMonth = 30 * 24 * 60 * 60;
        public const int OneDay = 24 * 60 * 60;

        public const string TopicName = "adnc_synchronize_topic";
        public const string LocalCaching = "l1";
        public const string RemoteCaching = "r1";
        public const string HybridCaching = "h1";
    }
}
