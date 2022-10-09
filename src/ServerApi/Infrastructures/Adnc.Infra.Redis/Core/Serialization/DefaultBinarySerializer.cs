using System.Runtime.Serialization.Formatters.Binary;

namespace Adnc.Infra.Redis.Core.Serialization
{
    /// <summary>
    /// Default binary formatter serializer.
    /// </summary>
    public class DefaultBinarySerializer : ISerializer
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => ConstValue.Serializer.DefaultBinarySerializerName;

        /// <summary>
        /// Deserialize the specified bytes.
        /// </summary>
        /// <returns>The deserialize.</returns>
        /// <param name="bytes">Bytes.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Deserialize<T>(byte[] bytes)
        {
            using var ms = new MemoryStream(bytes);
#pragma warning disable SYSLIB0011 // 类型或成员已过时
            return (T)new BinaryFormatter().Deserialize(ms);
#pragma warning restore SYSLIB0011 // 类型或成员已过时
        }

        /// <summary>
        /// Deserialize the specified bytes.
        /// </summary>
        /// <returns>The deserialize.</returns>
        /// <param name="bytes">Bytes.</param>
        /// <param name="type">Type.</param>
        public object Deserialize(byte[] bytes, Type type)
        {
            using var ms = new MemoryStream(bytes);
#pragma warning disable SYSLIB0011 // 类型或成员已过时
            return new BinaryFormatter().Deserialize(ms);
#pragma warning restore SYSLIB0011 // 类型或成员已过时
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="value">Value.</param>
        public object DeserializeObject(ArraySegment<byte> value)
        {
            using var ms = new MemoryStream(value.Array, value.Offset, value.Count);
#pragma warning disable SYSLIB0011 // 类型或成员已过时
            return new BinaryFormatter().Deserialize(ms);
#pragma warning restore SYSLIB0011 // 类型或成员已过时
        }

        /// <summary>
        /// Serialize the specified value.
        /// </summary>
        /// <returns>The serialize.</returns>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public byte[] Serialize<T>(T value)
        {
            using var ms = new MemoryStream();
#pragma warning disable SYSLIB0011 // 类型或成员已过时
            new BinaryFormatter().Serialize(ms, value);
#pragma warning restore SYSLIB0011 // 类型或成员已过时
            return ms.ToArray();
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="obj">Object.</param>
        public ArraySegment<byte> SerializeObject(object obj)
        {
            using var ms = new MemoryStream();
#pragma warning disable SYSLIB0011 // 类型或成员已过时
            new BinaryFormatter().Serialize(ms, obj);
#pragma warning restore SYSLIB0011 // 类型或成员已过时
            return new ArraySegment<byte>(ms.GetBuffer(), 0, (int)ms.Length);
        }
    }
}