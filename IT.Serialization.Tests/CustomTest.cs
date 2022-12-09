using IT.Generation;
using IT.Serialization.Tests.Data;

namespace IT.Serialization.Tests;

public class CustomTest
{
    protected static readonly IGenerator _generator = new Generation.KGySoft.Generator();
    protected static readonly City _city = _generator.Generate<City>();

    private static ITextSerialization<City> _serializer = new CitySerializer();

    class CitySerializer : TextSerialization<City>
    {
        public override City? Deserialize(ReadOnlyMemory<char> memory, CancellationToken cancellationToken)
        {
            var span = memory.Span;

            var sep = span.IndexOf('|');

            if (sep == -1) throw new FormatException();

            return new City
            {
                Name = span[..sep].TrimEnd().ToString(),
                Count = Int32.Parse(span[(sep + 1)..].TrimStart())
            };
        }

        public override string SerializeToText(City value, CancellationToken cancellationToken)
        {
            return $"{value.Name} | {value.Count}";
        }
    }

    [Test]
    public void Serializer()
    {
        var serialized = _serializer.Serialize(_city);

        Assert.NotNull(serialized);
        Assert.Greater(serialized.Length, 0);

        var city = _serializer.Deserialize(serialized);

        Assert.NotNull(city);

        Assert.True(_city.Equals(city));

        var text = _serializer.SerializeToText(_city);

        Assert.NotNull(text);
        Assert.Greater(text.Length, 0);

        city = _serializer.Deserialize(text.AsMemory());

        Assert.NotNull(city);

        Assert.True(_city.Equals(city));

        var path = @"C:\var\CustomTest.log";

        File.Delete(path);

        using var file = File.OpenWrite(path);
        _serializer.Serialize(file, _city);
        file.Close();


        using var reader = File.OpenRead(path);
        city = _serializer.Deserialize(reader);
        reader.Close();

        Assert.True(_city.Equals(city));
    }
}