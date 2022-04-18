using System;

namespace Adnc.Infra.Caching.Core.Serialization
{
    public class DefaultJsonSerializer : ICachingSerializer
    {
        public string Name => CachingConstValue.DefaultJsonSerializerName;

        public T Deserialize<T>(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(byte[] bytes, Type type)
        {
            throw new NotImplementedException();
        }

        public object DeserializeObject(ArraySegment<byte> value)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize<T>(T value)
        {
            throw new NotImplementedException();
        }

        public ArraySegment<byte> SerializeObject(object obj)
        {
            throw new NotImplementedException();
        }
    }
}