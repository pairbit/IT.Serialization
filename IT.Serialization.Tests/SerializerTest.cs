using IT.Generation;
using IT.Serialization.Tests.Data;

namespace IT.Serialization.Tests;

public abstract class SerializerTest
{
    protected static readonly IGenerator _generator = new Generation.KGySoft.Generator();
    protected static readonly Person _person = _generator.Generate<Person>();
    protected static readonly object _personObject = _generator.Generate(typeof(Person));

    private readonly ISerialization _serializer;
    private readonly ISerialization<Person> _serializerPerson;

    public SerializerTest(ISerialization serializer)
    {
        _serializer = serializer;
        _serializerPerson = new SerializationProxy<Person>(serializer);
    }

    protected virtual void Dump<T>(T obj, Byte[] bytes) { }

    [Test]
    public void SerializerGeneric()
    {
        var serialized = _serializer.Serialize(in _person);
        var serialized2 = _serializerPerson.Serialize(_person);

        Dump(_person, serialized);

        Assert.That(serialized, Is.Not.Null);
        Assert.That(serialized, Is.Not.Empty);
        Assert.That(serialized, Is.EqualTo(serialized2));

        var person = _serializer.Deserialize<Person>(serialized);
        var person2 = _serializerPerson.Deserialize(serialized2);

        Assert.That(person, Is.Not.Null);
        Assert.That(person, Is.EqualTo(person2));
        Assert.That(person, Is.EqualTo(_person));

        var path = Path.Combine(Path.GetTempPath(), "SerializerTest_Generic.log");
        var path2 = Path.Combine(Path.GetTempPath(), "SerializerTest_Generic2.log");

        File.Delete(path);
        File.Delete(path2);

        try
        {
            using var file = File.OpenWrite(path);
            _serializer.Serialize(_person, file);
            file.Close();

            using var file2 = File.OpenWrite(path2);
            _serializerPerson.Serialize(_person, file2);
            file2.Close();

            using var reader = File.OpenRead(path);
            person = _serializer.Deserialize<Person>(reader);
            reader.Close();

            using var reader2 = File.OpenRead(path2);
            person2 = _serializerPerson.Deserialize(reader2);
            reader.Close();

            Assert.That(person, Is.EqualTo(person2));
            Assert.That(person, Is.EqualTo(_person));

        }
        finally
        {
            File.Delete(path);
            File.Delete(path2);
        }
    }

    [Test]
    public void SerializerNonGeneric()
    {
        var serialized = _serializer.Serialize(typeof(Person), _personObject);

        Dump(_personObject, serialized);

        Assert.That(serialized, Is.Not.Null);
        Assert.That(serialized, Is.Not.Empty);

        var person = _serializer.Deserialize(typeof(Person), serialized);

        Assert.Multiple(() =>
        {
            Assert.That(person, Is.Not.Null);
            Assert.That(_personObject, Is.EqualTo(person));
        });

        var path = Path.Combine(Path.GetTempPath(), @"SerializerTest_NonGeneric.log");

        File.Delete(path);

        try
        {
            using var file = File.OpenWrite(path);
            _serializer.Serialize(_person, file);
            file.Close();

            using var reader = File.OpenRead(path);
            person = _serializer.Deserialize<Person>(reader);
            reader.Close();

            Assert.That(_person, Is.EqualTo(person));
        }
        finally
        {
            File.Delete(path);
        }
    }
}