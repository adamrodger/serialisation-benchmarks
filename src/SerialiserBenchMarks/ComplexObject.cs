using System;
using System.Collections.Generic;
using MessagePack;
using ProtoBuf;

namespace SerialiserBenchmarks
{
    [ProtoContract]
    [MessagePackObject]
    public class ComplexObject
    {
        [ProtoMember(1)] [Key(0)] public int Id { get; set; }
        [ProtoMember(2)] [Key(1)] public string Name { get; set; }
        [ProtoMember(3)] [Key(2)] public bool Enabled { get; set; }
        [ProtoMember(4)] [Key(3)] public DateTime LastUpdated { get; set; }
        [ProtoMember(5)] [Key(4)] public ICollection<SimpleObject> Children { get; set; }
        [ProtoMember(6)] [Key(5)] public IDictionary<int, string> Mappings { get; set;}
    }
}