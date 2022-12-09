namespace IT.Serialization;

public interface ITextSerialization<T> : ISerialization<T>, ITextSerializer<T>, ITextDeserializer<T>
{
}