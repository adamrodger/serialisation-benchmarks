using MessagePack;
using ProtoBuf;

namespace SerialiserBenchmarks
{
    [ProtoContract]
    [MessagePackObject]
    public class SimpleObject
    {
        [ProtoMember(1)] [Key(0)] public int Id { get; set; }
        [ProtoMember(2)] [Key(1)] public string Name { get; set; }
        [ProtoMember(3)] [Key(2)] public bool Enabled { get; set; }
    }
}