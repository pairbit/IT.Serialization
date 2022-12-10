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
        public override Int32 Deserialize(ReadOnlySpan<char> span, ref City? city)
        {
            var sep = span.IndexOf('|');

            if (sep == -1) throw new FormatException();

            if (city == null) city = new City();

            city.Name = span[..sep].TrimEnd().ToString();
            city.Count = Int32.Parse(span[(sep + 1)..].TrimStart());

            return span.Length;
        }

        public override String SerializeToText(in City? value)
        {
            if (value == null) return string.Empty;
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
        var chars = text.ToCharArray();

        Assert.NotNull(text);
        Assert.Greater(text.Length, 0);

        city = _serializer.Deserialize(text);

        Assert.True(_city.Equals(city));

        city = _serializer.Deserialize(chars);

        Assert.True(_city.Equals(city));

        var path = @"C:\var\CustomTest.log";

        File.Delete(path);

        using var file = File.OpenWrite(path);
        _serializer.Serialize(_city, file);
        file.Close();


        using var reader = File.OpenRead(path);
        city = _serializer.Deserialize(reader);
        reader.Close();

        Assert.True(_city.Equals(city));
    }
}