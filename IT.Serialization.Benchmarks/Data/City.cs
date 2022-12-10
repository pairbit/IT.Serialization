using System.Runtime.Serialization;

namespace IT.Serialization.Benchmarks.Data;

#if NETCOREAPP3_1_OR_GREATER
[global::MemoryPack.MemoryPackable]
#endif
[DataContract]
public partial record City
{
    [DataMember(Order = 0)]
    public string? Name { get; set; }

    [DataMember(Order = 1)]
    public int Count { get; set; }
}

//public class