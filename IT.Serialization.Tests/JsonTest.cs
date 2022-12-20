using System.Text.Json;
using System.Text.Json.Nodes;

namespace IT.Serialization.Tests;

public class JsonTest : TextSerializerTest
{
    //private static JsonSerializerOptions _options = new ();
    private static readonly Json.TextSerialization _serializer = new();

    public JsonTest() : base(_serializer) { }

    protected override void Dump<T>(T obj, byte[] bytes)
    {
        var reader = new Utf8JsonReader(bytes);

        var text = JsonNode.Parse(ref reader)?.ToString();

        Console.WriteLine(text);
    }
}