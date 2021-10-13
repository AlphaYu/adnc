using System;
using System.Text.Json;

namespace Adnc.Infra.Caching.Core.Serialization
{
    public class DefaultJsonSerializer : ICachingSerializer
    {
        public string Name => CachingConstValue.DefaultJsonSerializerName;

        public T Deserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes);
        }

        public object Deserialize(byte[] bytes, Type type)
        {
            return JsonSerializer.Deserialize<Type>(bytes);
        }

        public object DeserializeObject(ArraySegment<byte> value)
        {
            return JsonSerializer.Deserialize<object>(value);
        }

        public byte[] Serialize<T>(T value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value);
        }

        public ArraySegment<byte> SerializeObject(object obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }
    }
}