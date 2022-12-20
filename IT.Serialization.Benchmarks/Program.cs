using IT.Serialization.Benchmarks;

var bench = new SerializeBenchmark();

#if NETCOREAPP3_1_OR_GREATER

//var mp = bench.MemoryPack_Deserialize();
//var hp = bench.HyperSerializer_Deserialize();
//var bp = bench.BinaryPack_Deserialize();

//if (!hp!.Equals(mp)) throw new InvalidOperationException();
//if (!bp!.Equals(mp)) throw new InvalidOperationException();

#endif

var jp = bench.Json_Deserialize()!;

if (!jp.Equals(bench.MessagePack_Deserialize())) throw new InvalidOperationException();
if (!jp.Equals(bench.Utf8Json_Deserialize())) throw new InvalidOperationException();

BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(IT.Serialization.Benchmarks.SerializeBenchmark));

/*
 using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Hyper;
using MemoryPack;

var bench = new SerializeBenchmark();
var person = bench.person;

var p1 = bench.MemoryPack_Copy();
var p2 = bench.HyperSerializer_Copy();

if (!person.Equals(p1)) throw new InvalidOperationException();
if (!p1!.Equals(p2)) throw new InvalidOperationException("MemoryPack != HyperSerializer");

BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(SerializeBenchmark));

[MemoryPackable]
public partial record Person
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime Created { get; set; }
    public string? Mother { get; set; }
    public string? Father { get; set; }
}

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class SerializeBenchmark
{
    internal readonly Person person;
    private readonly byte[] _personBytes_MemoryPack;
    private readonly byte[] _personBytes_HyperSerializer;

    public SerializeBenchmark()
    {
        person = new Person
        {
            Id = new Guid("a4711a09-cc9b-4681-b13f-b5f46b61f5d4"),
            Name = "John",
            Age = 32,
            IsDeleted = false,
            Created = new DateTime(1990, 09, 08),
            Mother = "Inna",
            Father = "Mike"
        };

        _personBytes_MemoryPack = MemoryPack_Serialize();
        _personBytes_HyperSerializer = HyperSerializer_Serialize().ToArray();
    }

    [Benchmark]
    public byte[] MemoryPack_Serialize() => MemoryPackSerializer.Serialize(in person);

    [Benchmark]
    public Person? MemoryPack_Deserialize() => MemoryPackSerializer.Deserialize<Person>(_personBytes_MemoryPack);

    [Benchmark]
    public Person? MemoryPack_Copy() => MemoryPackSerializer.Deserialize<Person>(MemoryPackSerializer.Serialize(in person));

    [Benchmark]
    public Span<byte> HyperSerializer_Serialize() => HyperSerializer<Person>.Serialize(person);

    [Benchmark]
    public Person HyperSerializer_Deserialize() => HyperSerializer<Person>.Deserialize(_personBytes_HyperSerializer);

    [Benchmark]
    public Person HyperSerializer_Copy() => HyperSerializer<Person>.Deserialize(HyperSerializer<Person>.Serialize(person));
}
 */