using System.Runtime.Serialization;

namespace IT.Serialization.Benchmarks.Data;

[DataContract]
public record Address
{
    [DataMember(Order = 0)]
    public short Number { get; set; }

    [DataMember(Order = 1)]
    public string Street { get; set; }

    [DataMember(Order = 2)]
    public City City { get; set; }
}