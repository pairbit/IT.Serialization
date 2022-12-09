namespace IT.Serialization;

public interface ISerialization<T> : ISerializer<T>, IDeserializer<T>
{
}