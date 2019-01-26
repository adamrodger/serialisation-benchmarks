using MessagePack;
using MessagePack.Resolvers;

namespace SerialiserBenchmarks.Serialisers
{
    public class MessagePackContractless : ISerialiser
    {
        private readonly IFormatterResolver resolver = ContractlessStandardResolver.Instance;

        public byte[] Serialise<T>(T instance)
        {
            return MessagePackSerializer.Serialize(instance, resolver);
        }

        public T Deserialise<T>(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<T>(bytes, resolver);
        }
    }
}