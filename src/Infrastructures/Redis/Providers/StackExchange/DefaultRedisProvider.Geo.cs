using Adnc.Infra.Redis.Core;
using StackExchange.Redis;

namespace Adnc.Infra.Redis.Providers.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : IRedisProvider
    {
        public long GeoAdd(string cacheKey, List<(double longitude, double latitude, string member)> values)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(values, nameof(values));

            var list = new List<GeoEntry>();

            foreach (var item in values)
            {
                list.Add(new GeoEntry(item.longitude, item.latitude, item.member));
            }

            var res = _redisDb.GeoAdd(cacheKey, list.ToArray());
            return res;
        }

        public async Task<long> GeoAddAsync(string cacheKey, List<(double longitude, double latitude, string member)> values)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(values, nameof(values));

            var list = new List<GeoEntry>();

            foreach (var item in values)
            {
                list.Add(new GeoEntry(item.longitude, item.latitude, item.member));
            }

            var res = await _redisDb.GeoAddAsync(cacheKey, list.ToArray());
            return res;
        }

        public double? GeoDist(string cacheKey, string member1, string member2, string unit = "m")
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(member1, nameof(member1));
            ArgumentCheck.NotNullOrWhiteSpace(member2, nameof(member2));
            ArgumentCheck.NotNullOrWhiteSpace(unit, nameof(unit));

            var res = _redisDb.GeoDistance(cacheKey, member1, member2, GetGeoUnit(unit));
            return res;
        }

        public async Task<double?> GeoDistAsync(string cacheKey, string member1, string member2, string unit = "m")
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(member1, nameof(member1));
            ArgumentCheck.NotNullOrWhiteSpace(member2, nameof(member2));
            ArgumentCheck.NotNullOrWhiteSpace(unit, nameof(unit));

            var res = await _redisDb.GeoDistanceAsync(cacheKey, member1, member2, GetGeoUnit(unit));
            return res;
        }

        public List<string> GeoHash(string cacheKey, List<string> members)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(members, nameof(members));

            var list = new List<RedisValue>();
            foreach (var item in members)
            {
                list.Add(item);
            }

            var res = _redisDb.GeoHash(cacheKey, list.ToArray());
            return res.ToList();
        }

        public async Task<List<string>> GeoHashAsync(string cacheKey, List<string> members)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(members, nameof(members));

            var list = new List<RedisValue>();
            foreach (var item in members)
            {
                list.Add(item);
            }

            var res = await _redisDb.GeoHashAsync(cacheKey, list.ToArray());
            return res.ToList();
        }

        public List<(double longitude, double latitude)?> GeoPos(string cacheKey, List<string> members)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(members, nameof(members));

            var list = new List<RedisValue>();
            foreach (var item in members)
            {
                list.Add(item);
            }

            var res = _redisDb.GeoPosition(cacheKey, list.ToArray());

            var tuple = new List<(double longitude, double latitude)?>();

            foreach (var item in res)
            {
                if (item.HasValue)
                {
                    tuple.Add((item.Value.Longitude, item.Value.Latitude));
                }
                else
                {
                    tuple.Add(null);
                }
            }

            return tuple;
        }

        public async Task<List<(double longitude, double latitude)?>> GeoPosAsync(string cacheKey, List<string> members)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(members, nameof(members));

            var list = new List<RedisValue>();
            foreach (var item in members)
            {
                list.Add(item);
            }

            var res = await _redisDb.GeoPositionAsync(cacheKey, list.ToArray());

            var tuple = new List<(double longitude, double latitude)?>();

            foreach (var item in res)
            {
                if (item.HasValue)
                {
                    tuple.Add((item.Value.Longitude, item.Value.Latitude));
                }
                else
                {
                    tuple.Add(null);
                }
            }

            return tuple;
        }

        private GeoUnit GetGeoUnit(string unit)
        {
            GeoUnit geoUnit;
            switch (unit)
            {
                case "km":
                    geoUnit = GeoUnit.Kilometers;
                    break;

                case "ft":
                    geoUnit = GeoUnit.Feet;
                    break;

                case "mi":
                    geoUnit = GeoUnit.Miles;
                    break;

                default:
                    geoUnit = GeoUnit.Meters;
                    break;
            }
            return geoUnit;
        }
    }
}