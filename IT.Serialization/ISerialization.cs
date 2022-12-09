namespace IT.Serialization;

//https://stebet.net/real-world-example-of-reducing-allocations-using-span-t-and-memory-t/
public interface ISerialization : ISerializer, IDeserializer
{
}