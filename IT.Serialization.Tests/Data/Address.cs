using System.Runtime.Serialization;

namespace IT.Serialization.Tests.Data;

#if NET6_0_OR_GREATER
[global::MemoryPack.MemoryPackable]
#endif
[DataContract]
public partial record Address
{
    [DataMember(Order = 0)]
    public short Number { get; set; }

    [DataMember(Order = 1)]
    public string? Street { get; set; }

    [DataMember(Order = 2)]
    public City? City { get; set; }
}