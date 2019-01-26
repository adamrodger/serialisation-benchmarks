using MsgPack;
using MsgPack.Serialization;

namespace SerialiserBenchmarks.Serialisers
{
    public class MsgPackCliMap : ISerialiser
    {
        private readonly SerializationContext context = new SerializationContext
        {
            SerializationMethod = SerializationMethod.Map
        };

        public byte[] Serialise<T>(T instance)
        {
            return this.context.GetSerializer<T>().PackSingleObject(instance);
        }

        public T Deserialise<T>(byte[] bytes)
        {
            return this.context.GetSerializer<T>().UnpackSingleObject(bytes);
        }
    }
}