using System;
using System.Threading;

namespace IT.Serialization;

public class TextSerializationProxy<T> : SerializationProxy<T>, ITextSerialization<T>
{
    private readonly ITextSerialization _textSerialization;

    public TextSerializationProxy(ITextSerialization textSerialization) : base(textSerialization)
    {
        _textSerialization = textSerialization;
    }

    #region ITextSerialization

    //public virtual void Serialize(IBufferWriter<Char> writer, T value, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    public String SerializeToText(T value, CancellationToken cancellationToken)
        => _textSerialization.SerializeToText(value, cancellationToken);

    public T? Deserialize(ReadOnlyMemory<Char> memory, CancellationToken cancellationToken)
        => _textSerialization.Deserialize<T>(memory, cancellationToken);

    //public virtual T? Deserialize(in ReadOnlySequence<Char> sequence, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    #endregion ITextSerialization
}