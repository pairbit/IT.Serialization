using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using IT.Generation;
using IT.Serialization.Benchmarks.Data;

namespace IT.Serialization.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class SerializeBenchmark
{
    protected static readonly IGenerator _generator = new Generation.KGySoft.Generator();
    protected static readonly Person _person = _generator.Generate<Person>();
    protected static readonly Object _personObject = _generator.Generate(typeof(Person));

    private readonly ISerialization _jsonSerializer;
    private readonly byte[] _jsonSerializerBytes;
    private readonly ISerialization _messagePackSerializer;
    private readonly byte[] _messagePackSerializerBytes;
    private readonly ISerialization _utf8jsonSerializer;
    private readonly byte[] _utf8jsonSerializerBytes;

#if NETCOREAPP3_1_OR_GREATER

    private readonly ISerialization _memoryPackSerializer;
    private readonly byte[] _memoryPackSerializerBytes;

    //private readonly byte[] _hyperSerializerBytes;
    //private readonly byte[] _binaryPackBytes;

#endif

    public SerializeBenchmark()
    {
        _jsonSerializer = new Json.TextSerialization();
        _jsonSerializerBytes = Json_Serialize();
        _messagePackSerializer = new MessagePack.Serialization();
        _messagePackSerializerBytes = MessagePack_Serialize();
        _utf8jsonSerializer = new Utf8Json.TextSerialization();
        _utf8jsonSerializerBytes = Utf8Json_Serialize();

#if NETCOREAPP3_1_OR_GREATER
        _memoryPackSerializer = new MemoryPack.Serialization();
        _memoryPackSerializerBytes = MemoryPack_Serialize();

        //_hyperSerializerBytes = HyperSerializer_Serialize().ToArray();
        //_binaryPackBytes = BinaryPack_Serialize();
#endif
    }

    [Benchmark]
    public byte[] Json_Serialize() => _jsonSerializer.Serialize(_person);

    [Benchmark]
    public byte[] MessagePack_Serialize() => _messagePackSerializer.Serialize(_person);

    [Benchmark]
    public byte[] Utf8Json_Serialize() => _utf8jsonSerializer.Serialize(_person);

    [Benchmark]
    public Person? Json_Deserialize() => _jsonSerializer.Deserialize<Person>(_jsonSerializerBytes);

    [Benchmark]
    public Person? MessagePack_Deserialize() => _messagePackSerializer.Deserialize<Person>(_messagePackSerializerBytes);

    [Benchmark]
    public Person? Utf8Json_Deserialize() => _utf8jsonSerializer.Deserialize<Person>(_utf8jsonSerializerBytes);

    [Benchmark]
    public Person Json() => SerializeDeserialize(_jsonSerializer);

    [Benchmark]
    public Person MessagePack() => SerializeDeserialize(_messagePackSerializer);

    [Benchmark]
    public Person Utf8Json() => SerializeDeserialize(_utf8jsonSerializer);

#if NETCOREAPP3_1_OR_GREATER

    [Benchmark]
    public byte[] MemoryPack_Serialize() => global::MemoryPack.MemoryPackSerializer.Serialize(in _person);

    [Benchmark]
    public Person? MemoryPack_Deserialize() => global::MemoryPack.MemoryPackSerializer.Deserialize<Person>(_memoryPackSerializerBytes);

    [Benchmark]
    public byte[] IMemoryPack_Serialize() => _memoryPackSerializer.Serialize(in _person);

    [Benchmark]
    public Person? IMemoryPack_Deserialize() => _memoryPackSerializer.Deserialize<Person>(_memoryPackSerializerBytes);

    [Benchmark]
    public Person MemoryPack() => SerializeDeserialize(_memoryPackSerializer);

    //[Benchmark]
    //public Span<byte> HyperSerializer_Serialize() => Hyper.HyperSerializer<Person>.Serialize(_person);

    //[Benchmark]
    //public Person? HyperSerializer_Deserialize() => Hyper.HyperSerializer<Person>.Deserialize(_hyperSerializerBytes);

    //[Benchmark]
    //public byte[] BinaryPack_Serialize() => BinaryPack.BinaryConverter.Serialize(_person);

    //[Benchmark]
    //public Person? BinaryPack_Deserialize() => BinaryPack.BinaryConverter.Deserialize<Person?>(_binaryPackBytes);

#endif

    private Person SerializeDeserialize(ISerialization serializer)
    {
        var bytes = serializer.Serialize(_person);
        var person = serializer.Deserialize<Person>(bytes);
        if (!_person.Equals(person)) throw new InvalidOperationException();
        return person!;
    }
}