
namespace IT.Serialization.Tests;

public class JsonTest : TextSerializerTest
{
    //private static JsonSerializerOptions _options = new ();
    private static readonly Json.TextSerialization _serializer = new();

    public JsonTest() : base(_serializer) { }
}