namespace Adnc.Infra.Redis.Caching.Core
{
    /// <summary>
    /// Adnc.Infra.Redis const value.
    /// </summary>
    public static class CachingConstValue
    {
        public static class Serializer
        {
            public static readonly string DefaultBinarySerializerName = "binary";

            public static readonly string DefaultProtobufSerializerName = "proto";

            public static readonly string DefaultJsonSerializerName = "json";
        }

        public static class Provider
        {
            public static readonly string StackExchange = "StackExchange";
            public static readonly string FreeRedis = "FreeRedis";
            public static readonly string ServiceStack = "ServiceStack";
            public static readonly string CSRedis = "CSRedis";
        }

        public static readonly int PollyTimeout = 5;
    }
}