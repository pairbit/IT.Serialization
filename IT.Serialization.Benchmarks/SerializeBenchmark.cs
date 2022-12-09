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

    public SerializeBenchmark()
    {
        _jsonSerializer = new Json.TextSerialization();
        _jsonSerializerBytes = Json_Serialize();
        _messagePackSerializer = new MessagePack.Serialization();
        _messagePackSerializerBytes = MessagePack_Serialize();
        _utf8jsonSerializer = new Utf8Json.TextSerialization();
        _utf8jsonSerializerBytes = Utf8Json_Serialize();
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

    private Person SerializeDeserialize(ISerialization serializer)
    {
        var bytes = serializer.Serialize(_person);
        var person = serializer.Deserialize<Person>(bytes);
        //if (!_person.Equals(person)) throw new InvalidOperationException();
        return person!;
    }
}