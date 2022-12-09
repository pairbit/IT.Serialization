using System.Runtime.Serialization;

namespace IT.Serialization.Tests.Data;

[DataContract]
public record City
{
    [DataMember(Order = 0)]
    public string Name { get; set; }

    [DataMember(Order = 1)]
    public int Count { get; set; }
}

//public class