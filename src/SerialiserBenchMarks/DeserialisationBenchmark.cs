using System;
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
    public class DeserialisationBenchmark<T>
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
        private readonly byte[] messagePackDefault;
        private readonly byte[] messagePackContractless;
        private readonly byte[] msgPackArray;
        private readonly byte[] msgPackMap;
        private readonly byte[] json;
        private readonly byte[] protobufNet;

        public DeserialisationBenchmark()
        {
            var fixture = new Fixture();
            this.subject = fixture.Create<T>();

            this.messagePackDefault = MessagePackSerializer.Serialize(this.subject);
            this.messagePackContractless = MessagePackSerializer.Serialize(this.subject, ContractlessStandardResolver.Instance);
            this.msgPackArray = arrayContext.GetSerializer<T>().PackSingleObject(this.subject);
            this.msgPackMap = mapContext.GetSerializer<T>().PackSingleObject(this.subject);
            this.json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.subject));
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, this.subject);
                this.protobufNet = ms.ToArray();
            }
        }

        [GlobalCleanup]
        public void AfterAllTests()
        {
            Console.WriteLine("Message sizes in bytes:\n");
            Console.WriteLine($"\tMessagePack Default: {this.messagePackDefault.Length}");
            Console.WriteLine($"\tMessagePack ContractlessDefault: {this.messagePackContractless.Length}");
            Console.WriteLine($"\tMsgPack.Cli Array: {this.msgPackArray.Length}");
            Console.WriteLine($"\tMsgPack.Cli Map: {this.msgPackMap.Length}");
            Console.WriteLine($"\tNewtonsoft.Json: {this.json.Length}");
            Console.WriteLine($"\tprotobuf-net: {this.protobufNet.Length}");
        }

        [Benchmark]
        public T MessagePack_IntKey()
        {
            return MessagePackSerializer.Deserialize<T>(this.messagePackDefault);
        }

        [Benchmark]
        public T MessagePack_ContractlessDefault()
        {
            return MessagePackSerializer.Deserialize<T>(this.messagePackContractless);
        }

        [Benchmark]
        public T MsgPackCli_Array()
        {
            return arrayContext.GetSerializer<T>().UnpackSingleObject(this.msgPackArray);
        }

        [Benchmark]
        public T MsgPackCli_Map()
        {
            return arrayContext.GetSerializer<T>().UnpackSingleObject(this.msgPackMap);
        }

        [Benchmark]
        public T NewtonsoftJson()
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(this.json));
        }

        [Benchmark]
        public T ProtobufNet()
        {
            using (var ms = new MemoryStream(this.protobufNet))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}