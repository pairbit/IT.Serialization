using IT.Generation;
using IT.Serialization.Tests.Data;

namespace IT.Serialization.Tests;

public class CustomTest
{
    protected static readonly IGenerator _generator = new Generation.KGySoft.Generator();
    protected static readonly City _city = _generator.Generate<City>();

    private static readonly ITextSerialization<City> _serializer = new CitySerializer();

    class CitySerializer : TextSerialization<City>
    {
        public override Int32 Deserialize(ReadOnlySpan<char> span, ref City? city)
        {
            var sep = span.IndexOf('|');

            if (sep == -1) throw new FormatException();

            if (city == null) city = new City();

            city.Name = span[..sep].TrimEnd().ToString();

#if NETCOREAPP3_1_OR_GREATER
            city.Count = Int32.Parse(span[(sep + 1)..].TrimStart());
#else
            city.Count = Int32.Parse(span[(sep + 1)..].TrimStart().ToString());
#endif
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

        Assert.That(serialized, Is.Not.Null);
        Assert.That(serialized, Is.Not.Empty);

        var city = _serializer.Deserialize(serialized);

        Assert.That(city, Is.Not.Null);

        Assert.That(city, Is.EqualTo(_city));

        var text = _serializer.SerializeToText(_city);
        var chars = text.ToCharArray();

        Assert.That(text, Is.Not.Null);
        Assert.That(text, Is.Not.Empty);

        city = _serializer.Deserialize(text);

        Assert.That(city, Is.EqualTo(_city));

        city = _serializer.Deserialize(chars);

        Assert.That(city, Is.EqualTo(_city));

        var path = Path.Combine(Path.GetTempPath(), "CustomTest.log");

        File.Delete(path);

        try
        {
            using var file = File.OpenWrite(path);
            _serializer.Serialize(_city, file);
            file.Close();


            using var reader = File.OpenRead(path);
            city = _serializer.Deserialize(reader);
            reader.Close();

            Assert.That(city, Is.EqualTo(_city));
        }
        finally
        {
            File.Delete(path);
        }
    }
}