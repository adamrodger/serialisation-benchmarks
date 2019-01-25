using System.IO;
using System.Text;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using MessagePack.Resolvers;
using MsgPack.Serialization;
using Newtonsoft.Json;
using ProtoBuf;
using MessagePackSerializer = MessagePack.MessagePackSerializer;
using SerializationContext = MsgPack.Serialization.SerializationContext;

namespace SerialiserBenchmarks
{
    [ClrJob, CoreJob]
    public class SerialisationBenchmark<T>
    {
        // these need to be static because under real usage they're registered as singleton in DI
        private static readonly SerializationContext arrayContext = new SerializationContext
        {
            SerializationMethod = SerializationMethod.Array
        };

        private static readonly SerializationContext mapContext = new SerializationContext
        {
            SerializationMethod = SerializationMethod.Map
        };

        private readonly T subject;

        public SerialisationBenchmark()
        {
            var fixture = new Fixture();
            this.subject = fixture.Create<T>();
        }

        [Benchmark]
        public byte[] MessagePack_IntKey()
        {
            return MessagePackSerializer.Serialize(this.subject);
        }

        [Benchmark]
        public byte[] MessagePack_ContractlessDefault()
        {
            return MessagePackSerializer.Serialize(this.subject, ContractlessStandardResolver.Instance);
        }

        [Benchmark]
        public byte[] MsgPackCli_Array()
        {
            return arrayContext.GetSerializer<T>().PackSingleObject(this.subject);
        }

        [Benchmark]
        public byte[] MsgPackCli_Map()
        {
            return mapContext.GetSerializer<T>().PackSingleObject(this.subject);
        }

        [Benchmark]
        public byte[] NewtonsoftJson()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.subject));
        }

        [Benchmark]
        public byte[] ProtobufNet()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, this.subject);
                return ms.ToArray();
            }
        }
    }
}