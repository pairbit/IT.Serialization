namespace IT.Serialization;

public interface IAsyncSerialization<T> : IAsyncSerializer<T>, IAsyncDeserializer<T>
{
}