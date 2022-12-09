using IT.Serialization.Tests.Data;

namespace IT.Serialization.Tests;

public abstract class TextSerializerTest : SerializerTest
{
    private readonly ITextSerialization _textSerializer;
    private readonly ITextSerialization<Person> _textSerializerPerson;

    public TextSerializerTest(ITextSerialization textSerializer) : base(textSerializer)
    {
        _textSerializer = textSerializer;
        _textSerializerPerson = new TextSerializationProxy<Person>(textSerializer);
    }

    [Test]
    public void TextSerializerGeneric()
    {
        var serialized = _textSerializer.SerializeToText(_person);
        var serialized2 = _textSerializerPerson.SerializeToText(_person);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);
        Assert.That(serialized, Is.EqualTo(serialized2));

        var person = _textSerializer.Deserialize<Person>(serialized.AsMemory());
        var person2 = _textSerializer.Deserialize<Person>(serialized2.AsMemory());

        Assert.NotNull(person);

        Assert.That(person, Is.EqualTo(person2));
        Assert.That(person, Is.EqualTo(_person));
    }

    [Test]
    public void TextSerializerNonGeneric()
    {
        var serialized = _textSerializer.SerializeToText(typeof(Person), _personObject);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var person = _textSerializer.Deserialize(typeof(Person), serialized.AsMemory());

        Assert.NotNull(person);

        Assert.True(_personObject.Equals(person));
    }
}