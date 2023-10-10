namespace IT.Serialization.Generic;

public interface ISerialization<T> : ISerializer<T>, IDeserializer<T>
{
}