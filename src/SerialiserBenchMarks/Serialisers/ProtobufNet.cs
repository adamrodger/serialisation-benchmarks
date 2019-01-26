using System.IO;
using ProtoBuf;

namespace SerialiserBenchmarks.Serialisers
{
    public class ProtobufNet : ISerialiser
    {
        public byte[] Serialise<T>(T instance)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, instance);
                return ms.ToArray();
            }
        }

        public T Deserialise<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}