using MessagePack;
using System.Text.Json;

namespace IT.Serialization.Tests;

public class MessagePackTest : SerializerTest
{
    //private static JsonSerializerOptions _options = new ();
    private static MessagePack.Serialization _serializer = new();

    public MessagePackTest() : base(_serializer) { }

    protected override void Dump<T>(T obj, byte[] bytes)
    {
        var jsonArray = MessagePackSerializer.ConvertToJson(bytes);

        Console.WriteLine(jsonArray);

        var json = JsonSerializer.Serialize(obj);

        Console.WriteLine(json);

        var bdump = MessagePackSerializer.ConvertFromJson(jsonArray);

        var jsonArray2 = MessagePackSerializer.ConvertToJson(bdump);

        Console.WriteLine(jsonArray2);

        Console.WriteLine(MessagePackSerializer.SerializeToJson(obj));

        //if (!bdump.SequenceEqual(bytes))
        //     throw new InvalidOperationException();


    }
}