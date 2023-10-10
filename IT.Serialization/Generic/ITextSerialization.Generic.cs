namespace IT.Serialization.Generic;

public interface ITextSerialization<T> : ISerialization<T>, ITextSerializer<T>, ITextDeserializer<T>
{
}