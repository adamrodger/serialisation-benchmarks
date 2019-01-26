using MsgPack;
using MsgPack.Serialization;

namespace SerialiserBenchmarks.Serialisers
{
    public class MsgPackCliArray : ISerialiser
    {
        private readonly SerializationContext context = new SerializationContext
        {
            SerializationMethod = SerializationMethod.Array
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